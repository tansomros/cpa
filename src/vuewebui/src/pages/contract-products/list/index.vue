<script setup>
import { requiredValidator } from '@/@core/utils/validators'
import { toBuddhistYear } from '@/utils/dateUtils'

// eslint-disable-next-line import/extensions
import moment from 'moment/min/moment-with-locales.js'
import { computed, onMounted, ref } from 'vue'

moment.locale('th')

const searchQuery = ref('')
const itemsPerPage = ref(10)
const page = ref(1)
const sortBy = ref()
const orderBy = ref()
const selectedRows = ref([])
const isConfirmProgressDialogVisible = ref(false)
const progressDialogRef = ref(null)
const progressDialogFailureDescription = ref(null)

// State for Add/Edit Dialog
const isAddEditDialogVisible = ref(false)
const isEditMode = ref(false)
const editItemContractId = ref(null)
const editItemName = ref('')
const editItemQuantity = ref(0)
const editItemUnitId = ref(null)
const editItemUnitPrice = ref(0)
const selectedItem = ref(null)
const refForm = ref()
const isSaving = ref(false)

const headers = [
  {
    title: "ชื่อสินค้า/รายการ",
    key: "name",
  },
  {
    title: "จำนวน",
    key: "quantity",
  },
  {
    title: "หน่วยนับ",
    key: "unitId",
  },
  {
    title: "ราคาต่อหน่วย",
    key: "unitPrice",
  },
  {
    title: "ปรับปรุงล่าสุดเมื่อ",
    key: "lastModified",
  },
  {
    title: "จัดการ",
    key: "actions",
    sortable: false,
  },
]

// Fetch contract products
const { data: listData, execute: fetchData } = await useApi(`/ContractProducts/GetAllContractProductList`, { method: 'GET' })

const items = computed(() => listData.value?.contractProducts ?? [])
const totalItems = computed(() => items.value.length)

// Load units of measure
const { data: unitData } = await useApi(`/UnitOfMeasures/GetAllUnitOfMeasureList`, { method: 'GET' })
const unitsList = computed(() => unitData.value?.unitOfMeasures ?? [])
const unitsOptions = computed(() => unitsList.value.map(u => ({ title: u.name, value: u.id })))

const getUnitName = id => {
  const u = unitsList.value.find(x => x.id === id)
  return u ? u.name : '-'
}

const filteredItems = computed(() => {
  if (!searchQuery.value) return items.value
  const q = searchQuery.value.toLowerCase()
  return items.value.filter(i => i.name?.toLowerCase().includes(q))
})

// Delete logic
const selectedItemToDelete = ref(null)

const confirmDeleteItem = item => {
  selectedItemToDelete.value = item
  isConfirmProgressDialogVisible.value = true
}

const cancelDeleteItem = () => {
  selectedItemToDelete.value = null
}

const deleteItem = async id => {
  try {
    progressDialogRef.value?.setProgress()
    await $api(`/ContractProducts/${id}`, { method: 'DELETE' })
    progressDialogRef.value?.setSuccess()
    fetchData()
  } catch {
    progressDialogFailureDescription.value = 'ไม่สามารถลบข้อมูลสินค้าได้'
    progressDialogRef.value?.setFailure()
  }
}

const updateOptions = options => {
  sortBy.value = options.sortBy?.[0]?.key
  orderBy.value = options.sortBy?.[0]?.order
}

// Highlighting state (30 seconds)
const highlightedItems = ref(new Map())

const loadHighlights = () => {
  try {
    const data = JSON.parse(sessionStorage.getItem('contract_products_highlights') || '[]')
    const map = new Map()
    const now = Date.now()
    data.forEach(([id, info]) => {
      const elapsed = now - info.timestamp
      if (elapsed < 30000) {
        map.set(id, info)
        setTimeout(() => {
          if (highlightedItems.value.has(id)) {
            highlightedItems.value.delete(id)
            highlightedItems.value = new Map(highlightedItems.value)
            saveHighlights()
          }
        }, 30000 - elapsed)
      }
    })
    return map
  } catch {
    return new Map()
  }
}

const saveHighlights = () => {
  sessionStorage.setItem('contract_products_highlights', JSON.stringify(Array.from(highlightedItems.value.entries())))
}

const highlightItem = (id, type) => {
  highlightedItems.value.set(id, { timestamp: Date.now(), type })
  highlightedItems.value = new Map(highlightedItems.value)
  saveHighlights()
  
  setTimeout(() => {
    if (highlightedItems.value.has(id)) {
      highlightedItems.value.delete(id)
      highlightedItems.value = new Map(highlightedItems.value)
      saveHighlights()
    }
  }, 30000)
}

const openAddDialog = () => {
  isEditMode.value = false
  editItemContractId.value = null
  editItemName.value = ''
  editItemQuantity.value = 0
  editItemUnitId.value = null
  editItemUnitPrice.value = 0
  selectedItem.value = null
  isAddEditDialogVisible.value = true
}

const openEditDialog = item => {
  isEditMode.value = true
  selectedItem.value = item
  editItemContractId.value = item.contractId
  editItemName.value = item.name
  editItemQuantity.value = item.quantity
  editItemUnitId.value = item.unitId
  editItemUnitPrice.value = item.unitPrice
  isAddEditDialogVisible.value = true
}

const closeAddEditDialog = () => {
  isAddEditDialogVisible.value = false
}

// State for View Dialog
const isViewDialogVisible = ref(false)
const viewItem = ref(null)

const openViewDialog = item => {
  viewItem.value = item
  isViewDialogVisible.value = true
}

const closeViewDialog = () => {
  isViewDialogVisible.value = false
}

const saveItem = async () => {
  if (!refForm.value) return
  const isValid = await refForm.value.validate()
  if (!isValid.valid) return

  isSaving.value = true
  try {
    const payload = {
      contractId: parseInt(editItemContractId.value) || 0,
      name: editItemName.value,
      quantity: parseInt(editItemQuantity.value) || 0,
      unitId: editItemUnitId.value,
      unitPrice: parseFloat(editItemUnitPrice.value) || 0
    }
    if (isEditMode.value) {
      // Edit mode
      await $api(`/ContractProducts/${selectedItem.value.id}`, {
        method: 'PUT',
        body: {
          id: selectedItem.value.id,
          ...payload
        }
      })
      highlightItem(selectedItem.value.id, 'edit')
    } else {
      // Add mode
      const response = await $api(`/ContractProducts`, {
        method: 'POST',
        body: payload
      })
      if (response) {
        highlightItem(response, 'create')
      }
    }
    isAddEditDialogVisible.value = false
    fetchData()
  } catch (error) {
    console.error('Error saving contract product:', error)
  } finally {
    isSaving.value = false
  }
}

const getRowProps = ({ item }) => {
  const highlightInfo = highlightedItems.value.get(item.id)
  if (!highlightInfo) return {}
  
  const elapsed = Date.now() - highlightInfo.timestamp
  if (elapsed >= 30000) {
    return {}
  }
  
  const remainingSeconds = ((30000 - elapsed) / 1000).toFixed(1)
  
  return {
    class: highlightInfo.type === 'create' ? 'highlighted-row-create' : 'highlighted-row-edit',
    style: `animation: highlight-fade-${highlightInfo.type} ${remainingSeconds}s linear forwards;`
  }
}

onMounted(() => {
  highlightedItems.value = loadHighlights()
})
</script>

<template>
  <section>
    <VCard class="mb-6">
      <VCardItem class="pb-4">
        <VCardTitle>รายการข้อมูลสินค้า/รายการ</VCardTitle>
      </VCardItem>

      <VCardText class="d-flex flex-wrap gap-4">
        <div class="me-3 d-flex gap-3">
          <AppSelect :model-value="itemsPerPage" :items="[
            { value: 10, title: '10' },
            { value: 25, title: '25' },
            { value: 50, title: '50' },
            { value: 100, title: '100' },
          ]" style="inline-size: 6.25rem;" @update:model-value="itemsPerPage = parseInt($event, 10)" />
        </div>
        <VSpacer />

        <div class="app-user-search-filter d-flex align-center flex-wrap gap-4">
          <!-- 👉 Search  -->
          <div style="inline-size: 15.625rem;">
            <AppTextField v-model="searchQuery" placeholder="ค้นหา" />
          </div>

          <!-- 👉 Add button -->
          <VBtn prepend-icon="tabler-plus" @click="openAddDialog">
            เพิ่มข้อมูลสินค้า
          </VBtn>
        </div>
      </VCardText>

      <VDivider />

      <!-- SECTION datatable -->
      <VDataTableServer v-model:items-per-page="itemsPerPage" v-model:model-value="selectedRows" v-model:page="page"
        :items="filteredItems" item-value="id" :items-length="filteredItems.length" :headers="headers" class="text-no-wrap"
        show-select @update:options="updateOptions" :row-props="getRowProps">
        <template #item.name="{ item }">
          <div v-if="item.name">
            {{ item.name }}
          </div>
        </template>
        <template #item.quantity="{ item }">
          <div>
            {{ item.quantity?.toLocaleString() }}
          </div>
        </template>
        <template #item.unitId="{ item }">
          <div>
            {{ getUnitName(item.unitId) }}
          </div>
        </template>
        <template #item.unitPrice="{ item }">
          <div>
            {{ item.unitPrice?.toLocaleString() }}
          </div>
        </template>
        <template #item.lastModified="{ item }">
          <div v-if="item.lastModified">
            {{ toBuddhistYear(moment(item.lastModified), "LLL") }}
          </div>
        </template>
        <!-- Actions -->
        <template #item.actions="{ item }">
          <IconBtn @click="openViewDialog(item)">
            <VIcon icon="tabler-eye" />
          </IconBtn>

          <IconBtn @click="openEditDialog(item)">
            <VIcon icon="tabler-pencil" />
          </IconBtn>

          <IconBtn @click="confirmDeleteItem(item)">
            <VIcon icon="tabler-trash" color="error" />
          </IconBtn>

          <VBtn icon variant="text" color="medium-emphasis">
            <VIcon icon="tabler-dots-vertical" />
            <VMenu activator="parent">
              <VList>
                <VListItem @click="openViewDialog(item)">
                  <template #prepend>
                    <VIcon icon="tabler-eye" />
                  </template>
                  <VListItemTitle>แสดง</VListItemTitle>
                </VListItem>

                <VListItem @click="openEditDialog(item)">
                  <template #prepend>
                    <VIcon icon="tabler-pencil" />
                  </template>
                  <VListItemTitle>แก้ไข</VListItemTitle>
                </VListItem>

                <VListItem @click="confirmDeleteItem(item)">
                  <template #prepend>
                    <VIcon icon="tabler-trash" />
                  </template>
                  <VListItemTitle>ลบ</VListItemTitle>
                </VListItem>
              </VList>
            </VMenu>
          </VBtn>
        </template>

        <!-- pagination -->
        <template #bottom>
          <TablePagination v-model:page="page" :items-per-page="itemsPerPage" :total-items="filteredItems.length" />
        </template>
      </VDataTableServer>

      <ConfirmProgressDialog ref="progressDialogRef" v-model:model-value="isConfirmProgressDialogVisible"
        confirm-title="โปรดยืนยันการลบข้อมูล" confirm-message="ข้อมูลที่เกี่ยวข้องจะถูกลบทั้งหมด"
        progress-message="กำลังประมวลผล, โปรดรอสักครู่..." success-title="ลบข้อมูลสำเร็จ!"
        success-message="ลบข้อมูลสำเร็จ" failure-title="ลบข้อมูลไม่สำเร็จ"
        failure-message="ลบข้อมูลไม่สำเร็จ, โปรดตรวจสอบ!" :failure-data="progressDialogFailureDescription"
        @confirm="deleteItem(selectedItemToDelete.id)" @retry="deleteItem(selectedItemToDelete.id)"
        @cancel="cancelDeleteItem" />

      <!-- 👉 Add/Edit Dialog -->
      <VDialog v-model="isAddEditDialogVisible" max-width="600">
        <VCard class="pa-2 pa-sm-10">
          <DialogCloseBtn class="custom-close-btn" @click="closeAddEditDialog" />
          <VCardText>
            <h4 class="text-h4 text-center mb-2">
              {{ isEditMode ? 'แก้ไขข้อมูลสินค้า' : 'เพิ่มข้อมูลสินค้า' }}
            </h4>
            <p class="text-body-1 text-center mb-6">
              {{ isEditMode ? 'แก้ไขรายละเอียดข้อมูลสินค้า' : 'เพิ่มข้อมูลสินค้าใหม่' }}
            </p>
            <VForm ref="refForm" @submit.prevent="saveItem">
              <VRow>
                <VCol cols="12">
                  <AppTextField v-model="editItemContractId" label="รหัสสัญญา (Contract ID)" placeholder="กรอกรหัสสัญญา" type="number" :rules="[requiredValidator]" autofocus />
                </VCol>
                <VCol cols="12">
                  <AppTextField v-model="editItemName" label="ชื่อสินค้า/รายการ" placeholder="กรอกชื่อสินค้า/รายการ" :rules="[requiredValidator]" />
                </VCol>
                <VCol cols="6">
                  <AppTextField v-model="editItemQuantity" label="จำนวน" placeholder="กรอกจำนวน" type="number" :rules="[requiredValidator]" />
                </VCol>
                <VCol cols="6">
                  <AppSelect v-model="editItemUnitId" label="หน่วยนับ" placeholder="เลือกหน่วยนับ" :items="unitsOptions" :rules="[requiredValidator]" />
                </VCol>
                <VCol cols="12">
                  <AppTextField v-model="editItemUnitPrice" label="ราคาต่อหน่วย (บาท)" placeholder="กรอกราคาต่อหน่วย" type="number" :rules="[requiredValidator]" />
                </VCol>
                <VCol cols="12" class="d-flex justify-center gap-4 mt-4">
                  <VBtn type="submit" :loading="isSaving">บันทึก</VBtn>
                  <VBtn color="secondary" variant="tonal" @click="closeAddEditDialog">ยกเลิก</VBtn>
                </VCol>
              </VRow>
            </VForm>
          </VCardText>
        </VCard>
      </VDialog>

      <!-- 👉 View Dialog -->
      <VDialog v-model="isViewDialogVisible" max-width="600">
        <VCard class="pa-2 pa-sm-10">
          <DialogCloseBtn class="custom-close-btn" @click="closeViewDialog" />
          <VCardText>
            <h4 class="text-h4 text-center mb-2">
              รายละเอียดข้อมูลสินค้า
            </h4>
            <p class="text-body-1 text-center mb-6">
              รายละเอียดข้อมูลสินค้าทั้งหมด
            </p>
            <VList v-if="viewItem" class="card-list">
              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  รหัสสัญญา (Contract ID)
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.contractId }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  ชื่อสินค้า/รายการ
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.name }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  จำนวน
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.quantity?.toLocaleString() }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  หน่วยนับ
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ getUnitName(viewItem.unitId) }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  ราคาต่อหน่วย (บาท)
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ viewItem.unitPrice?.toLocaleString() }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  ปรับปรุงล่าสุดเมื่อ
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">
                    {{ viewItem.lastModified ? toBuddhistYear(moment(viewItem.lastModified), "LLL") : '-' }}
                  </span>
                </template>
              </VListItem>
            </VList>
            <div class="d-flex justify-center gap-4 mt-6">
              <VBtn color="secondary" variant="tonal" @click="closeViewDialog">ปิด</VBtn>
            </div>
          </VCardText>
        </VCard>
      </VDialog>
    </VCard>
  </section>
</template>

<style lang="scss">
@keyframes highlight-fade-create {
  0% {
    background-color: rgba(40, 199, 111, 25%);
  }

  100% {
    background-color: transparent;
  }
}

@keyframes highlight-fade-edit {
  0% {
    background-color: rgba(255, 224, 178, 50%);
  }

  100% {
    background-color: transparent;
  }
}

.highlighted-row-create td {
  animation: highlight-fade-create 30s linear forwards !important;
}

.highlighted-row-edit td {
  animation: highlight-fade-edit 30s linear forwards !important;
}

.custom-close-btn {
  border-radius: 50% !important;
  background-color: rgba(var(--v-theme-on-surface), 0.08) !important;
  box-shadow: none !important;
  color: rgba(var(--v-theme-on-surface), 0.6) !important;
  inset-block-start: 1rem !important;
  inset-inline-end: 1rem !important;
  transform: none !important;
  transition: all 0.25s ease-in-out !important;

  &:hover {
    background-color: rgba(var(--v-theme-error), 0.15) !important;
    box-shadow: none !important;
    color: rgb(var(--v-theme-error)) !important;
    transform: rotate(90deg) scale(1.15) !important;
  }
}
</style>
