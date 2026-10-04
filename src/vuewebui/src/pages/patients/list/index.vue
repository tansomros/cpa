<script setup>
import { requiredValidator } from '@/@core/utils/validators'
import { $api } from '@/utils/api'
import { onMounted, ref } from 'vue'

const searchQuery = ref('')
const itemsPerPage = ref(10)
const page = ref(1)
const items = ref([])
const totalItems = ref(0)
const isLoading = ref(false)
const errorMessage = ref('')
const prefixes = ref([])

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

const genderOptions = [
  { value: 'ชาย', title: 'ชาย' },
  { value: 'หญิง', title: 'หญิง' },
]

const bloodGroupOptions = ['A', 'B', 'AB', 'O'].map(value => ({ value, title: value }))

const emptyForm = () => ({
  hospitalNumber: '',
  prefix: null,
  firstName: '',
  middleName: '',
  lastName: '',
  gender: null,
  birthDate: '',
  nationId: '',
  bloodGroup: null,
  telephoneNumber: '',
  address: '',
  zipCode: '',
  drugAllergy: '',
  chronicDisease: '',
})

const form = ref(emptyForm())

const headers = [
  { title: 'HN', key: 'hospitalNumber' },
  { title: 'ชื่อ-สกุล', key: 'fullName' },
  { title: 'เพศ', key: 'gender' },
  { title: 'วันเกิด', key: 'birthDate' },
  { title: 'เลขบัตรประชาชน', key: 'nationId' },
  { title: 'เบอร์โทร', key: 'telephoneNumber' },
  { title: 'จัดการ', key: 'actions', sortable: false },
]

const hospitalNumberRules = [
  requiredValidator,
  v => !v || v.length <= 8 || 'HN ต้องไม่เกิน 8 ตัวอักษร',
]

const describeError = (error, fallback) => {
  const errors = error?.data?.errors
  if (errors && typeof errors === 'object')
    return Object.values(errors).flat().join(' ')

  return error?.data?.detail || error?.data?.title || fallback
}

const fetchPrefixes = async () => {
  const result = await $api('/prefixs', { method: 'GET', query: { page: 1, limit: 200 } })

  prefixes.value = (result.items ?? []).map(item => ({ value: item.name, title: item.name }))
}

const fetchItems = async () => {
  isLoading.value = true
  errorMessage.value = ''

  const query = { page: page.value, limit: itemsPerPage.value }
  if (searchQuery.value.trim())
    query.search = searchQuery.value.trim()

  try {
    const result = await $api('/patients', { method: 'GET', query })

    items.value = result.items ?? []
    totalItems.value = result.totalCount ?? 0
  } catch {
    items.value = []
    totalItems.value = 0
    errorMessage.value = 'โหลดรายการผู้รับบริการไม่สำเร็จ'
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
    await $api(`/patients/${id}`, { method: 'DELETE' })
    progressDialogRef.value?.stopProgress()
    progressDialogRef.value?.showSuccess()
    await fetchItems()
  } catch (error) {
    progressDialogFailureDescription.value = { message: [describeError(error, 'ไม่สามารถลบผู้รับบริการได้')] }
    progressDialogRef.value?.stopProgress()
    progressDialogRef.value?.showFailure()
  }
}

const toForm = item => ({
  hospitalNumber: item?.hospitalNumber ?? '',
  prefix: item?.prefix ?? null,
  firstName: item?.firstName ?? '',
  middleName: item?.middleName ?? '',
  lastName: item?.lastName ?? '',
  gender: item?.gender ?? null,
  birthDate: item?.birthDate ? String(item.birthDate).slice(0, 10) : '',
  nationId: item?.nationId ?? '',
  bloodGroup: item?.bloodGroup ?? null,
  telephoneNumber: item?.telephoneNumber ?? '',
  address: item?.address ?? '',
  zipCode: item?.zipCode ?? '',
  drugAllergy: item?.drugAllergy ?? '',
  chronicDisease: item?.chronicDisease ?? '',
})

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
  form.value = toForm(item)
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
    hospitalNumber: form.value.hospitalNumber.trim(),
    prefix: form.value.prefix,
    firstName: form.value.firstName.trim(),
    middleName: form.value.middleName.trim(),
    lastName: form.value.lastName.trim(),
    gender: form.value.gender,
    birthDate: form.value.birthDate,
    nationId: form.value.nationId.trim() || null,
    bloodGroup: form.value.bloodGroup || null,
    telephoneNumber: form.value.telephoneNumber.trim() || null,
    address: form.value.address.trim() || null,
    zipCode: form.value.zipCode.trim() || null,
    drugAllergy: form.value.drugAllergy.trim() || null,
    chronicDisease: form.value.chronicDisease.trim() || null,
  }

  try {
    if (isEditMode.value) {
      await $api(`/patients/${selectedItem.value.id}`, {
        method: 'PUT',
        body: { id: selectedItem.value.id, ...body },
      })
    }
    else {
      await $api('/patients', { method: 'POST', body })
    }

    isAddEditDialogVisible.value = false
    await fetchItems()
  } catch (error) {
    saveError.value = describeError(error, 'เกิดข้อผิดพลาดในการบันทึกข้อมูล')
  } finally {
    isSaving.value = false
  }
}

onMounted(fetchPrefixes)
fetchItems()
</script>

<template>
  <section>
    <VCard class="mb-6">
      <VCardItem class="pb-4">
        <VCardTitle>รายการผู้รับบริการ</VCardTitle>
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
            เพิ่มผู้รับบริการ
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
        confirm-message="ข้อมูลผู้รับบริการรายนี้จะถูกลบ"
        progress-message="กำลังประมวลผล, โปรดรอสักครู่..."
        success-title="ลบข้อมูลสำเร็จ!"
        success-message="ลบผู้รับบริการสำเร็จ"
        failure-title="ลบข้อมูลไม่สำเร็จ"
        failure-message="ลบข้อมูลไม่สำเร็จ, โปรดตรวจสอบ!"
        :failure-data="progressDialogFailureDescription"
        @confirm="deleteItem(selectedItemToDelete.id)"
        @retry="deleteItem(selectedItemToDelete.id)"
        @cancel="cancelDeleteItem"
      />

      <VDialog
        v-model="isAddEditDialogVisible"
        max-width="900"
      >
        <VCard class="pa-2 pa-sm-8">
          <DialogCloseBtn @click="isAddEditDialogVisible = false" />
          <VCardText>
            <h4 class="text-h4 text-center mb-6">
              {{ isEditMode ? 'แก้ไขผู้รับบริการ' : 'เพิ่มผู้รับบริการ' }}
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
                  md="4"
                >
                  <AppTextField
                    v-model="form.hospitalNumber"
                    label="HN"
                    :rules="hospitalNumberRules"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppSelect
                    v-model="form.prefix"
                    :items="prefixes"
                    label="คำนำหน้าชื่อ"
                    :rules="[requiredValidator]"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppSelect
                    v-model="form.gender"
                    :items="genderOptions"
                    label="เพศ"
                    :rules="[requiredValidator]"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppTextField
                    v-model="form.firstName"
                    label="ชื่อ"
                    :rules="[requiredValidator]"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppTextField
                    v-model="form.middleName"
                    label="ชื่อกลาง"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppTextField
                    v-model="form.lastName"
                    label="นามสกุล"
                    :rules="[requiredValidator]"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppTextField
                    v-model="form.birthDate"
                    label="วันเกิด"
                    type="date"
                    :rules="[requiredValidator]"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppTextField
                    v-model="form.nationId"
                    label="เลขบัตรประชาชน"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppSelect
                    v-model="form.bloodGroup"
                    :items="bloodGroupOptions"
                    label="หมู่เลือด"
                    clearable
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="6"
                >
                  <AppTextField
                    v-model="form.telephoneNumber"
                    label="เบอร์โทร"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="6"
                >
                  <AppTextField
                    v-model="form.zipCode"
                    label="รหัสไปรษณีย์"
                  />
                </VCol>
                <VCol cols="12">
                  <AppTextField
                    v-model="form.address"
                    label="ที่อยู่"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="6"
                >
                  <AppTextField
                    v-model="form.drugAllergy"
                    label="แพ้ยา"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="6"
                >
                  <AppTextField
                    v-model="form.chronicDisease"
                    label="โรคประจำตัว"
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
        max-width="720"
      >
        <VCard class="pa-2 pa-sm-8">
          <DialogCloseBtn @click="isViewDialogVisible = false" />
          <VCardText>
            <h4 class="text-h4 text-center mb-6">
              รายละเอียดผู้รับบริการ
            </h4>
            <VList v-if="viewItem">
              <VListItem title="HN" :subtitle="viewItem.hospitalNumber || '-'" />
              <VListItem title="ชื่อ-สกุล" :subtitle="viewItem.fullName || '-'" />
              <VListItem title="เพศ" :subtitle="viewItem.gender || '-'" />
              <VListItem title="วันเกิด" :subtitle="viewItem.birthDate || '-'" />
              <VListItem title="เลขบัตรประชาชน" :subtitle="viewItem.nationId || '-'" />
              <VListItem title="หมู่เลือด" :subtitle="viewItem.bloodGroup || '-'" />
              <VListItem title="เบอร์โทร" :subtitle="viewItem.telephoneNumber || '-'" />
              <VListItem title="ที่อยู่" :subtitle="viewItem.address || '-'" />
              <VListItem title="แพ้ยา" :subtitle="viewItem.drugAllergy || '-'" />
              <VListItem title="โรคประจำตัว" :subtitle="viewItem.chronicDisease || '-'" />
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
