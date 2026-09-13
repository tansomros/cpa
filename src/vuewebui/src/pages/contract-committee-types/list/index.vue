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
const selectedItem = ref()
const selectedRows = ref([])
const isConfirmProgressDialogVisible = ref(false)
const progressDialogRef = ref(null)
const progressDialogFailureDescription = ref(null)

// State for Add/Edit Dialog
const isAddEditDialogVisible = ref(false)
const isEditMode = ref(false)
const editItemName = ref('')
const editItemStatus = ref(true)
const refForm = ref()
const isSaving = ref(false)

// Highlighting state
const highlightedItems = ref(new Map())

const loadHighlights = () => {
  try {
    const data = JSON.parse(sessionStorage.getItem('contract_committee_types_highlights') || '[]')
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
  sessionStorage.setItem('contract_committee_types_highlights', JSON.stringify(Array.from(highlightedItems.value.entries())))
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

const headers = [
  { title: 'ชื่อ', key: 'name' },
  { title: 'สถานะ', key: 'statusFlag', width: 100 },
  { title: "ปรับปรุงล่าสุดเมื่อ",key: "lastModified",width: 100 },
  { title: 'จัดการ', key: 'actions', sortable: false, width: 120 },
]

const { data: listData, execute: fetchData } = await useApi(`/ContractCommitteeTypes/GetAllContractCommitteeTypeList`, { method: 'GET' })
const items = computed(() => listData.value?.contractCommitteeTypes ?? [])
const filteredItems = computed(() => {
  if (!searchQuery.value) return items.value
  const q = searchQuery.value.toLowerCase()
  return items.value.filter(i => i.name?.toLowerCase().includes(q))
})

const openAddDialog = () => {
  isEditMode.value = false
  editItemName.value = ''
  editItemStatus.value = true
  selectedItem.value = null
  isAddEditDialogVisible.value = true
}

const openEditDialog = item => {
  isEditMode.value = true
  selectedItem.value = item
  editItemName.value = item.name
  editItemStatus.value = item.statusFlag
  isAddEditDialogVisible.value = true
}

const closeAddEditDialog = () => {
  isAddEditDialogVisible.value = false
}

const saveItem = async () => {
  if (!refForm.value) return
  const isValid = await refForm.value.validate()
  if (!isValid.valid) return

  isSaving.value = true
  try {
    if (isEditMode.value) {
      // Edit mode
      await $api(`/ContractCommitteeTypes/${selectedItem.value.id}`, {
        method: 'PUT',
        body: {
          id: selectedItem.value.id,
          name: editItemName.value,
          statusFlag: editItemStatus.value
        }
      })
      highlightItem(selectedItem.value.id, 'edit')
    } else {
      // Add mode
      const response = await $api(`/ContractCommitteeTypes`, {
        method: 'POST',
        body: {
          name: editItemName.value
        }
      })
      if (response) {
        highlightItem(response, 'create')
      }
    }
    isAddEditDialogVisible.value = false
    fetchData()
  } catch (error) {
    console.error('Error saving contract committee type:', error)
  } finally {
    isSaving.value = false
  }
}

const deleteItem = async id => {
  try {
    progressDialogRef.value?.setProgress()
    await $api(`/ContractCommitteeTypes/${id}`, { method: 'DELETE' })
    progressDialogRef.value?.setSuccess()
    fetchData()
  } catch {
    progressDialogFailureDescription.value = 'ไม่สามารถลบข้อมูลได้'
    progressDialogRef.value?.setFailure()
  }
}
const confirmDelete = item => { selectedItem.value = item; isConfirmProgressDialogVisible.value = true }
const cancelDelete = () => { selectedItem.value = null }
</script>

<template>
  <section>
    <VCard class="mb-6">
      <VCardItem class="pb-4"><VCardTitle>รายการประเภทคณะกรรมการ</VCardTitle></VCardItem>
      <VDivider />
      <VCardText class="d-flex flex-wrap gap-4">
        <div class="me-3 d-flex gap-3">
          <AppSelect :model-value="itemsPerPage" :items="
            [{ value: 10, title: '10' },
            { value: 25, title: '25' },
            { value: 50, title: '50' },
            { value: 100, title: '100' }
            ]" style="inline-size: 6.25rem;" @update:model-value="itemsPerPage = parseInt($event, 10)" />
        </div>
        <VSpacer />
        <div class="app-user-search-filter d-flex align-center flex-wrap gap-4">
          <div style="inline-size: 15.625rem;"><AppTextField v-model="searchQuery" placeholder="ค้นหา" /></div>
          <VBtn prepend-icon="tabler-plus" @click="openAddDialog">เพิ่มข้อมูล</VBtn>
        </div>
      </VCardText>
      <VDivider />
      <VDataTableServer v-model:items-per-page="itemsPerPage" v-model:model-value="selectedRows" v-model:page="page" :items="filteredItems" item-value="id" :items-length="filteredItems.length" :headers="headers" class="text-no-wrap" show-select :row-props="getRowProps">
        <template #item.statusFlag="{ item }">
          <VChip :color="item.statusFlag ? 'success' : 'error'" size="small" label>{{ item.statusFlag ? 'ใช้งาน' : 'ไม่ใช้งาน' }}</VChip>
        </template>
        <template #item.lastModified="{ item }">
          <div v-if="item.lastModified">
            {{ toBuddhistYear(moment(item.lastModified), "LLL") }}
          </div>
        </template>
        <template #item.actions="{ item }">
          <!--<RouterLink :to="{ name: 'contract-committee-types-view-id', params: { id: item.id } }">
          <IconBtn><VIcon icon="tabler-eye" /></IconBtn></RouterLink>-->
          <IconBtn @click="openEditDialog(item)"><VIcon icon="tabler-pencil" /></IconBtn>
          <IconBtn @click="confirmDelete(item)"><VIcon icon="tabler-trash" color="error" /></IconBtn>
        </template>
        <template #bottom><TablePagination v-model:page="page" :items-per-page="itemsPerPage" :total-items="filteredItems.length" /></template>
      </VDataTableServer>
      
      <!-- 👉 Confirm Delete Dialog -->
      <ConfirmProgressDialog ref="progressDialogRef" v-model:model-value="isConfirmProgressDialogVisible" confirm-title="โปรดยืนยันการลบข้อมูล" confirm-message="ข้อมูลที่เกี่ยวข้องจะถูกลบทั้งหมด" progress-message="กำลังประมวลผล, โปรดรอสักครู่..." success-title="ลบข้อมูลสำเร็จ!" success-message="ลบข้อมูลสำเร็จ" failure-title="ลบข้อมูลไม่สำเร็จ" failure-message="ลบข้อมูลไม่สำเร็จ, โปรดตรวจสอบ!" :failure-data="progressDialogFailureDescription" @confirm="deleteItem(selectedItem.id)" @retry="deleteItem(selectedItem.id)" @cancel="cancelDelete" />

      <!-- 👉 Add/Edit Dialog -->
      <VDialog v-model="isAddEditDialogVisible" max-width="600">
        <VCard class="pa-2 pa-sm-10">
          <DialogCloseBtn @click="closeAddEditDialog" />
          <VCardText>
            <h4 class="text-h4 text-center mb-2">
              {{ isEditMode ? 'แก้ไขประเภทคณะกรรมการ' : 'เพิ่มประเภทคณะกรรมการ' }}
            </h4>
            <p class="text-body-1 text-center mb-6">
              {{ isEditMode ? 'แก้ไขข้อมูลประเภทคณะกรรมการ' : 'เพิ่มข้อมูลประเภทคณะกรรมการใหม่' }}
            </p>
            <VForm ref="refForm" @submit.prevent="saveItem">
              <VRow>
                <VCol cols="12">
                  <AppTextField v-model="editItemName" label="ชื่อประเภทคณะกรรมการ" placeholder="กรอกชื่อประเภทคณะกรรมการ" :rules="[requiredValidator]" autofocus />
                </VCol>
                <VCol cols="12" v-if="isEditMode">
                  <VSwitch v-model="editItemStatus" label="สถานะการใช้งาน" color="success" :true-value="true" :false-value="false" />
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
    // background-color: rgba(115, 103, 240, 25%);
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
</style>
