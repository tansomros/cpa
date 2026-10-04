<script setup>
import { requiredValidator } from '@/@core/utils/validators'
import { $api } from '@/utils/api'
import { computed, onMounted, ref } from 'vue'

const searchQuery = ref('')
const itemsPerPage = ref(10)
const page = ref(1)
const items = ref([])
const totalItems = ref(0)
const isLoading = ref(false)
const errorMessage = ref('')
const roles = ref([])
const pharmacies = ref([])

const isConfirmProgressDialogVisible = ref(false)
const progressDialogRef = ref(null)
const progressDialogFailureDescription = ref(null)
const selectedItemToDelete = ref(null)

const isAddEditDialogVisible = ref(false)
const isEditMode = ref(false)
const isViewDialogVisible = ref(false)
const viewItem = ref(null)
const refForm = ref()
const isSaving = ref(false)
const saveError = ref('')
const selectedItem = ref(null)

const emptyForm = () => ({
  username: '',
  password: '',
  displayName: '',
  positionName: '',
  email: '',
  pharmacyId: null,
  roleId: null,
  isActive: true,
})

const form = ref(emptyForm())

const headers = [
  { title: 'ชื่อผู้ใช้', key: 'username' },
  { title: 'ชื่อที่แสดง', key: 'displayName' },
  { title: 'ตำแหน่ง', key: 'positionName' },
  { title: 'บทบาท', key: 'role' },
  { title: 'ร้านยา', key: 'pharmacyId' },
  { title: 'สถานะ', key: 'isActive' },
  { title: 'จัดการ', key: 'actions', sortable: false },
]

const passwordRules = computed(() => {
  const lengthRule = v => !v || v.length >= 4 || 'รหัสผ่านต้องมีอย่างน้อย 4 ตัวอักษร'

  return isEditMode.value ? [lengthRule] : [requiredValidator, lengthRule]
})

const describeError = (error, fallback) => {
  const errors = error?.data?.errors
  if (errors && typeof errors === 'object')
    return Object.values(errors).flat().join(' ')

  return error?.data?.detail || error?.data?.title || fallback
}

const pharmacyName = id => pharmacies.value.find(item => item.value === id)?.title || '-'

const fetchLookups = async () => {
  const [roleResult, pharmacyResult] = await Promise.allSettled([
    $api('/roles', { method: 'GET', query: { page: 1, limit: 100 } }),
    $api('/pharmacies', { method: 'GET', query: { page: 1, limit: 100 } }),
  ])

  if (roleResult.status === 'fulfilled')
    roles.value = (roleResult.value.items ?? []).map(item => ({ value: item.id, title: item.name }))

  if (pharmacyResult.status === 'fulfilled')
    pharmacies.value = (pharmacyResult.value.items ?? []).map(item => ({ value: item.id, title: item.name }))
}

const fetchItems = async () => {
  isLoading.value = true
  errorMessage.value = ''

  const query = { page: page.value, limit: itemsPerPage.value }
  if (searchQuery.value.trim())
    query.search = searchQuery.value.trim()

  try {
    const result = await $api('/users', { method: 'GET', query })

    items.value = result.items ?? []
    totalItems.value = result.totalCount ?? 0
  } catch {
    items.value = []
    totalItems.value = 0
    errorMessage.value = 'โหลดรายการผู้ใช้งานไม่สำเร็จ'
  } finally {
    isLoading.value = false
  }
}

let searchTimer

watch(searchQuery, () => {
  clearTimeout(searchTimer)
  searchTimer = setTimeout(() => {
    if (page.value === 1)
      fetchItems()
    else
      page.value = 1
  }, 300)
})

watch(itemsPerPage, () => {
  if (page.value === 1)
    fetchItems()
  else
    page.value = 1
})

watch(page, fetchItems)

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
    await $api(`/users/${id}`, { method: 'DELETE' })
    progressDialogRef.value?.stopProgress()
    progressDialogRef.value?.showSuccess()
    await fetchItems()
  } catch (error) {
    progressDialogFailureDescription.value = { message: [describeError(error, 'ไม่สามารถลบผู้ใช้งานได้')] }
    progressDialogRef.value?.stopProgress()
    progressDialogRef.value?.showFailure()
  }
}

const openAddDialog = () => {
  isEditMode.value = false
  selectedItem.value = null
  form.value = emptyForm()
  saveError.value = ''
  isAddEditDialogVisible.value = true
}

const openEditDialog = item => {
  isEditMode.value = true
  selectedItem.value = item
  form.value = {
    username: item.username ?? '',
    password: '',
    displayName: item.displayName ?? '',
    positionName: item.positionName ?? '',
    email: item.email ?? '',
    pharmacyId: item.pharmacyId ?? null,
    roleId: item.roleId ?? null,
    isActive: item.isActive ?? true,
  }
  saveError.value = ''
  isAddEditDialogVisible.value = true
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
    username: form.value.username.trim(),
    displayName: form.value.displayName.trim(),
    positionName: form.value.positionName.trim(),
    email: form.value.email.trim() || null,
    pharmacyId: form.value.pharmacyId || null,
    roleId: form.value.roleId,
    isActive: form.value.isActive,
  }

  if (!isEditMode.value || form.value.password)
    body.password = form.value.password

  try {
    if (isEditMode.value) {
      await $api(`/users/${selectedItem.value.id}`, {
        method: 'PUT',
        body: { id: selectedItem.value.id, ...body },
      })
    }
    else {
      await $api('/users', { method: 'POST', body })
    }

    isAddEditDialogVisible.value = false
    await fetchItems()
  } catch (error) {
    saveError.value = describeError(error, 'เกิดข้อผิดพลาดในการบันทึกข้อมูล')
  } finally {
    isSaving.value = false
  }
}

onMounted(fetchLookups)
fetchItems()
</script>

<template>
  <section>
    <VCard class="mb-6">
      <VCardItem class="pb-4">
        <VCardTitle>รายการผู้ใช้งาน</VCardTitle>
      </VCardItem>

      <VCardText class="d-flex flex-wrap gap-4">
        <div class="me-3 d-flex gap-3">
          <AppSelect
            :model-value="itemsPerPage"
            :items="[
              { value: 10, title: '10' },
              { value: 25, title: '25' },
              { value: 50, title: '50' },
              { value: 100, title: '100' },
            ]"
            style="inline-size: 6.25rem;"
            @update:model-value="itemsPerPage = parseInt($event, 10)"
          />
        </div>
        <VSpacer />

        <div class="d-flex align-center flex-wrap gap-4">
          <div style="inline-size: 15.625rem;">
            <AppTextField
              v-model="searchQuery"
              placeholder="ค้นหา"
            />
          </div>
          <VBtn
            prepend-icon="tabler-plus"
            @click="openAddDialog"
          >
            เพิ่มผู้ใช้งาน
          </VBtn>
        </div>
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
        item-value="id"
        :items-length="totalItems"
        :loading="isLoading"
        :headers="headers"
        class="text-no-wrap"
      >
        <template #item.pharmacyId="{ item }">
          {{ pharmacyName(item.pharmacyId) }}
        </template>
        <template #item.isActive="{ item }">
          <VChip
            :color="item.isActive ? 'success' : 'secondary'"
            size="small"
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
        confirm-message="ผู้ใช้งานรายนี้จะถูกลบออกจากระบบ"
        progress-message="กำลังประมวลผล, โปรดรอสักครู่..."
        success-title="ลบข้อมูลสำเร็จ!"
        success-message="ลบผู้ใช้งานสำเร็จ"
        failure-title="ลบข้อมูลไม่สำเร็จ"
        failure-message="ลบข้อมูลไม่สำเร็จ, โปรดตรวจสอบ!"
        :failure-data="progressDialogFailureDescription"
        @confirm="deleteItem(selectedItemToDelete.id)"
        @retry="deleteItem(selectedItemToDelete.id)"
        @cancel="cancelDeleteItem"
      />

      <VDialog
        v-model="isAddEditDialogVisible"
        max-width="720"
      >
        <VCard class="pa-2 pa-sm-10">
          <DialogCloseBtn @click="isAddEditDialogVisible = false" />
          <VCardText>
            <h4 class="text-h4 text-center mb-6">
              {{ isEditMode ? 'แก้ไขผู้ใช้งาน' : 'เพิ่มผู้ใช้งาน' }}
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
                <VCol
                  cols="12"
                  md="6"
                >
                  <AppTextField
                    v-model="form.username"
                    label="ชื่อผู้ใช้"
                    :rules="[requiredValidator]"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="6"
                >
                  <AppTextField
                    v-model="form.password"
                    label="รหัสผ่าน"
                    type="password"
                    :placeholder="isEditMode ? 'เว้นว่างหากไม่เปลี่ยนรหัสผ่าน' : ''"
                    :rules="passwordRules"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="6"
                >
                  <AppTextField
                    v-model="form.displayName"
                    label="ชื่อที่แสดง"
                    :rules="[requiredValidator]"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="6"
                >
                  <AppTextField
                    v-model="form.positionName"
                    label="ตำแหน่ง"
                    :rules="[requiredValidator]"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="6"
                >
                  <AppTextField
                    v-model="form.email"
                    label="อีเมล"
                    type="email"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="6"
                >
                  <AppSelect
                    v-model="form.roleId"
                    :items="roles"
                    label="บทบาท"
                    :rules="[requiredValidator]"
                  />
                </VCol>
                <VCol cols="12">
                  <AppSelect
                    v-model="form.pharmacyId"
                    :items="pharmacies"
                    label="ร้านยา"
                    clearable
                  />
                </VCol>
                <VCol cols="12">
                  <VSwitch
                    v-model="form.isActive"
                    label="เปิดใช้งาน"
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
                    @click="isAddEditDialogVisible = false"
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
        max-width="640"
      >
        <VCard class="pa-2 pa-sm-10">
          <DialogCloseBtn @click="isViewDialogVisible = false" />
          <VCardText>
            <h4 class="text-h4 text-center mb-6">
              รายละเอียดผู้ใช้งาน
            </h4>
            <VList v-if="viewItem">
              <VListItem title="ชื่อผู้ใช้" :subtitle="viewItem.username" />
              <VListItem title="ชื่อที่แสดง" :subtitle="viewItem.displayName" />
              <VListItem title="ตำแหน่ง" :subtitle="viewItem.positionName || '-'" />
              <VListItem title="อีเมล" :subtitle="viewItem.email || '-'" />
              <VListItem title="บทบาท" :subtitle="viewItem.role || '-'" />
              <VListItem title="ร้านยา" :subtitle="pharmacyName(viewItem.pharmacyId)" />
              <VListItem title="สถานะ" :subtitle="viewItem.isActive ? 'ใช้งาน' : 'ไม่ใช้งาน'" />
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
  </section>
</template>
