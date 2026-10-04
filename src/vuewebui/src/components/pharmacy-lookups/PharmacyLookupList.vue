<script setup>
import { requiredValidator } from '@/@core/utils/validators'
import { $api } from '@/utils/api'

const props = defineProps({
  title: {
    type: String,
    required: true,
  },
  apiPath: {
    type: String,
    required: true,
  },
})

const searchQuery = ref('')
const itemsPerPage = ref(10)
const page = ref(1)
const items = ref([])
const totalItems = ref(0)
const isLoading = ref(false)
const errorMessage = ref('')

const isDialogVisible = ref(false)
const isEditMode = ref(false)
const isSaving = ref(false)
const saveError = ref('')
const refForm = ref()
const form = ref(emptyForm())

const isConfirmProgressDialogVisible = ref(false)
const progressDialogRef = ref(null)
const progressDialogFailureDescription = ref(null)
const selectedItemToDelete = ref(null)

const isViewDialogVisible = ref(false)
const viewItem = ref(null)

const headers = [
  { title: 'รหัส', key: 'code', sortable: false },
  { title: 'ชื่อ', key: 'name', sortable: false },
  { title: 'คำอธิบาย', key: 'description', sortable: false },
  { title: 'ลำดับ', key: 'sort', sortable: false },
  { title: 'สถานะ', key: 'isActive', sortable: false },
  { title: 'จัดการ', key: 'actions', sortable: false },
]

const codeRules = [requiredValidator]
const nameRules = [requiredValidator]

function emptyForm() {
  return {
    id: null,
    code: '',
    name: '',
    description: '',
    sort: 0,
    isActive: true,
  }
}

const fetchItems = async () => {
  isLoading.value = true
  errorMessage.value = ''

  const query = { page: page.value, limit: itemsPerPage.value }
  if (searchQuery.value.trim())
    query.search = searchQuery.value.trim()

  try {
    const result = await $api(props.apiPath, { method: 'GET', query })

    items.value = result.items ?? []
    totalItems.value = result.totalCount ?? 0
  } catch {
    items.value = []
    totalItems.value = 0
    errorMessage.value = `โหลดรายการ${props.title}ไม่สำเร็จ`
  } finally {
    isLoading.value = false
  }
}

let searchTimer

watch(searchQuery, () => {
  clearTimeout(searchTimer)
  searchTimer = setTimeout(() => {
    page.value = 1
    fetchItems()
  }, 300)
})

watch(itemsPerPage, () => {
  page.value = 1
  fetchItems()
})

watch(page, fetchItems)

const openAddDialog = () => {
  isEditMode.value = false
  form.value = emptyForm()
  saveError.value = ''
  isDialogVisible.value = true
}

const openEditDialog = item => {
  isEditMode.value = true
  form.value = {
    id: item.id,
    code: item.code ?? '',
    name: item.name ?? '',
    description: item.description ?? '',
    sort: item.sort ?? 0,
    isActive: item.isActive ?? true,
  }
  saveError.value = ''
  isDialogVisible.value = true
}

const closeDialog = () => {
  isDialogVisible.value = false
}

const openViewDialog = item => {
  viewItem.value = item
  isViewDialogVisible.value = true
}

const saveItem = async () => {
  const validation = await refForm.value?.validate()
  if (!validation?.valid)
    return

  isSaving.value = true
  saveError.value = ''

  const body = {
    code: form.value.code,
    name: form.value.name,
    description: form.value.description || null,
    sort: Number(form.value.sort) || 0,
    isActive: form.value.isActive,
  }

  try {
    if (isEditMode.value) {
      await $api(`${props.apiPath}/${form.value.id}`, {
        method: 'PUT',
        body: { id: form.value.id, ...body },
      })
    } else {
      await $api(props.apiPath, { method: 'POST', body })
    }

    isDialogVisible.value = false
    await fetchItems()
  } catch (error) {
    saveError.value = error?.data?.detail || error?.data?.title || 'บันทึกข้อมูลไม่สำเร็จ'
  } finally {
    isSaving.value = false
  }
}

const confirmDeleteItem = item => {
  selectedItemToDelete.value = item
  isConfirmProgressDialogVisible.value = true
}

const cancelDeleteItem = () => {
  selectedItemToDelete.value = null
}

const deleteItem = async id => {
  try {
    progressDialogRef.value?.startProgress()
    await $api(`${props.apiPath}/${id}`, { method: 'DELETE' })
    progressDialogRef.value?.stopProgress()
    progressDialogRef.value?.showSuccess()
    await fetchItems()
  } catch {
    progressDialogFailureDescription.value = { message: [`ไม่สามารถลบ${props.title}ได้`] }
    progressDialogRef.value?.stopProgress()
    progressDialogRef.value?.showFailure()
  }
}

fetchItems()
</script>

<template>
  <VCard>
    <VCardItem class="pb-4">
      <VCardTitle>รายการ{{ title }}</VCardTitle>
    </VCardItem>

    <VCardText class="d-flex flex-wrap gap-4">
      <AppSelect
        v-model="itemsPerPage"
        :items="[10, 25, 50, 100]"
        style="inline-size: 6.25rem;"
      />
      <VSpacer />
      <div style="inline-size: 15.625rem;">
        <AppTextField
          v-model="searchQuery"
          placeholder="ค้นหารหัสหรือชื่อ"
        />
      </div>
      <VBtn
        prepend-icon="tabler-plus"
        @click="openAddDialog"
      >
        เพิ่ม{{ title }}
      </VBtn>
    </VCardText>

    <VAlert
      v-if="errorMessage"
      type="error"
      variant="tonal"
      class="mx-6 mb-4"
    >
      {{ errorMessage }}
    </VAlert>

    <VDivider />

    <VDataTableServer
      v-model:items-per-page="itemsPerPage"
      v-model:page="page"
      :items="items"
      :items-length="totalItems"
      :headers="headers"
      :loading="isLoading"
      item-value="id"
      class="text-no-wrap"
    >
      <template #item.description="{ item }">
        {{ item.description || '-' }}
      </template>
      <template #item.isActive="{ item }">
        <VChip
          :color="item.isActive ? 'success' : 'error'"
          size="small"
          label
        >
          {{ item.isActive ? 'ใช้งาน' : 'ไม่ใช้งาน' }}
        </VChip>
      </template>
      <template #item.actions="{ item }">
        <IconBtn @click="openViewDialog(item)">
          <VIcon icon="tabler-eye" />
        </IconBtn>
        <IconBtn @click="openEditDialog(item)">
          <VIcon icon="tabler-pencil" />
        </IconBtn>
        <IconBtn @click="confirmDeleteItem(item)">
          <VIcon
            icon="tabler-trash"
            color="error"
          />
        </IconBtn>
      </template>
      <template #bottom>
        <TablePagination
          v-model:page="page"
          :items-per-page="itemsPerPage"
          :total-items="totalItems"
        />
      </template>
    </VDataTableServer>

    <ConfirmProgressDialog
      ref="progressDialogRef"
      v-model:model-value="isConfirmProgressDialogVisible"
      confirm-title="โปรดยืนยันการลบข้อมูล"
      confirm-message="ข้อมูลที่เกี่ยวข้องจะถูกลบทั้งหมด"
      progress-message="กำลังประมวลผล, โปรดรอสักครู่..."
      success-title="ลบข้อมูลสำเร็จ!"
      success-message="ลบข้อมูลสำเร็จ"
      failure-title="ลบข้อมูลไม่สำเร็จ"
      failure-message="ลบข้อมูลไม่สำเร็จ, โปรดตรวจสอบ!"
      :failure-data="progressDialogFailureDescription"
      @confirm="deleteItem(selectedItemToDelete.id)"
      @retry="deleteItem(selectedItemToDelete.id)"
      @cancel="cancelDeleteItem"
    />

    <VDialog
      v-model="isDialogVisible"
      max-width="600"
    >
      <VCard class="pa-2 pa-sm-8">
        <VCardText>
          <h4 class="text-h4 text-center mb-6">
            {{ isEditMode ? `แก้ไข${title}` : `เพิ่ม${title}` }}
          </h4>
          <VForm
            ref="refForm"
            @submit.prevent="saveItem"
          >
            <VRow>
              <VCol
                v-if="saveError"
                cols="12"
              >
                <VAlert
                  type="error"
                  variant="tonal"
                >
                  {{ saveError }}
                </VAlert>
              </VCol>
              <VCol cols="12">
                <AppTextField
                  v-model="form.code"
                  label="รหัส"
                  :rules="codeRules"
                />
              </VCol>
              <VCol cols="12">
                <AppTextField
                  v-model="form.name"
                  label="ชื่อ"
                  :rules="nameRules"
                />
              </VCol>
              <VCol cols="12">
                <AppTextField
                  v-model="form.description"
                  label="คำอธิบาย"
                />
              </VCol>
              <VCol cols="12">
                <AppTextField
                  v-model="form.sort"
                  label="ลำดับ"
                  type="number"
                />
              </VCol>
              <VCol cols="12">
                <VSwitch
                  v-model="form.isActive"
                  label="ใช้งาน"
                />
              </VCol>
              <VCol
                cols="12"
                class="d-flex justify-center gap-4"
              >
                <VBtn
                  type="submit"
                  :loading="isSaving"
                >
                  บันทึก
                </VBtn>
                <VBtn
                  color="secondary"
                  variant="tonal"
                  @click="closeDialog"
                >
                  ยกเลิก
                </VBtn>
              </VCol>
            </VRow>
          </VForm>
        </VCardText>
      </VCard>
    </VDialog>

    <VDialog
      v-model="isViewDialogVisible"
      max-width="600"
    >
      <VCard class="pa-2 pa-sm-8">
        <VCardText>
          <h4 class="text-h4 text-center mb-6">
            รายละเอียด{{ title }}
          </h4>
          <VList v-if="viewItem">
            <VListItem>
              <VListItemTitle>รหัส</VListItemTitle>
              <template #append>
                <span>{{ viewItem.code }}</span>
              </template>
            </VListItem>
            <VListItem>
              <VListItemTitle>ชื่อ</VListItemTitle>
              <template #append>
                <span>{{ viewItem.name }}</span>
              </template>
            </VListItem>
            <VListItem>
              <VListItemTitle>คำอธิบาย</VListItemTitle>
              <template #append>
                <span>{{ viewItem.description || '-' }}</span>
              </template>
            </VListItem>
            <VListItem>
              <VListItemTitle>ลำดับ</VListItemTitle>
              <template #append>
                <span>{{ viewItem.sort }}</span>
              </template>
            </VListItem>
            <VListItem>
              <VListItemTitle>สถานะ</VListItemTitle>
              <template #append>
                <span>{{ viewItem.isActive ? 'ใช้งาน' : 'ไม่ใช้งาน' }}</span>
              </template>
            </VListItem>
          </VList>
          <div class="d-flex justify-center mt-6">
            <VBtn
              color="secondary"
              variant="tonal"
              @click="isViewDialogVisible = false"
            >
              ปิด
            </VBtn>
          </div>
        </VCardText>
      </VCard>
    </VDialog>
  </VCard>
</template>
