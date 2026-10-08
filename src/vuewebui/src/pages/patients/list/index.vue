<script setup>
import { requiredValidator } from '@/@core/utils/validators'
import { $api } from '@/utils/api'
import { createThaiDatePickerConfig, formatThaiDate, startOfToday, THAI_DATE_PLACEHOLDER, yearsAgo } from '@/utils/thaiDate'

import moment from 'moment/min/moment-with-locales.js'
moment.locale('th')

const searchQuery = ref('')
const itemsPerPage = ref(10)
const page = ref(1)
const items = ref([])
const totalItems = ref(0)
const isLoading = ref(false)
const errorMessage = ref('')

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

// Birthday: shown and typed as B.E. dd/mm/yyyy, kept in the form and sent to the API
// as C.E. "yyyy-MM-dd" (null when empty). Allowed range: today back to 120 years ago.
const BIRTH_DATE_MAX_AGE_YEARS = 120
const birthDateError = ref('')

const birthDatePickerConfig = createThaiDatePickerConfig({
  minDate: yearsAgo(BIRTH_DATE_MAX_AGE_YEARS),
  maxDate: startOfToday(),
  onError: message => {
    birthDateError.value = message
  },
})

const genderOptions = [
  { value: 'M', title: 'ชาย' },
  { value: 'F', title: 'หญิง' },
]

// Smoking and drinking dropdowns come from the SmartEnum lookups at /options/{category}
// (see LookupRegistry). Each option is { value: code, title: Thai display name }.
const lifestyleOptions = ref({
  'smoking': [],
  'cigarette-type': [],
  'drinking': [],
})

const isLifestyleOptionsLoading = ref(false)

const loadLifestyleOptions = async () => {
  const missing = Object.keys(lifestyleOptions.value).filter(category => !lifestyleOptions.value[category].length)
  if (!missing.length)
    return

  isLifestyleOptionsLoading.value = true
  try {
    await Promise.all(missing.map(async category => {
      try {
        const result = await $api(`/options/${category}`, { method: 'GET' })

        lifestyleOptions.value[category] = (result ?? []).map(o => ({
          value: o.value,
          title: o.displayName,
        }))
      } catch {
        lifestyleOptions.value[category] = []
      }
    }))
  } finally {
    isLifestyleOptionsLoading.value = false
  }
}

const provinces = ref([])
const isProvincesLoading = ref(false)

const loadProvinces = async () => {
  if (provinces.value.length)
    return

  isProvincesLoading.value = true
  try {
    const result = await $api('/provinces', { method: 'GET', query: { page: 1, limit: 200 } })

    provinces.value = (result.items ?? []).map(p => ({
      value: p.id,
      title: p.name,
    }))
  } catch {
    provinces.value = []
  } finally {
    isProvincesLoading.value = false
  }
}

const districts = ref([])
const subDistricts = ref([])
const isDistrictsLoading = ref(false)
const isSubDistrictsLoading = ref(false)

const loadDistricts = async provinceId => {
  districts.value = []
  if (!provinceId)
    return

  isDistrictsLoading.value = true
  try {
    const result = await $api('/districts', {
      method: 'GET',
      query: { page: 1, limit: 200, provinceId },
    })

    districts.value = (result.items ?? []).map(d => ({
      value: d.id,
      title: d.name,
    }))
  } catch {
    districts.value = []
  } finally {
    isDistrictsLoading.value = false
  }
}

const loadSubDistricts = async districtId => {
  subDistricts.value = []
  if (!districtId)
    return

  isSubDistrictsLoading.value = true
  try {
    const result = await $api('/sub-districts', {
      method: 'GET',
      query: { page: 1, limit: 200, districtId },
    })

    subDistricts.value = (result.items ?? []).map(s => ({
      value: s.subDistrictId,
      title: s.name,
      zipCode: s.zipCode,
    }))
  } catch {
    subDistricts.value = []
  } finally {
    isSubDistrictsLoading.value = false
  }
}

const onProvinceChange = async provinceId => {
  form.value.provinceId = provinceId
  form.value.districtId = null
  form.value.subDistrictId = null
  subDistricts.value = []
  await loadDistricts(provinceId)
}

const onDistrictChange = async districtId => {
  form.value.districtId = districtId
  form.value.subDistrictId = null
  await loadSubDistricts(districtId)
}

const onSubDistrictChange = subDistrictId => {
  form.value.subDistrictId = subDistrictId

  const match = subDistricts.value.find(s => s.value === subDistrictId)
  if (match?.zipCode && !form.value.zipCode)
    form.value.zipCode = match.zipCode
}

const prepareAddressLookups = async () => {
  districts.value = []
  subDistricts.value = []

  if (form.value.provinceId) {
    await loadDistricts(form.value.provinceId)
    if (form.value.districtId)
      await loadSubDistricts(form.value.districtId)
  }
}

const emptyForm = () => ({
  foreName: '',
  surname: '',
  gender: null,
  birthDate: '',
  cardId: '',
  telephone: '',
  timeContact: '',
  addressType: '',
  addressNo: '',
  road: '',
  provinceId: null,
  districtId: null,
  subDistrictId: null,
  zipCode: '',
  mainClaim: '',
  isActive: true,
  education: '',
  occupation: '',
  isAllergy: false,
  drugAllergy: '',
  isSmoke: false,
  smoke: null,
  smokeYear: '',
  smokeCigarette: '',
  cigaretteType: null,
  smokingQuit: false,
  smokingRemark: '',
  alcohol: null,
  alcoholFQ: '',
})

const form = ref(emptyForm())

const headers = [
  { title: 'รหัส', key: 'id' },
  { title: 'ชื่อ-สกุล', key: 'fullName' },
  { title: 'เพศ', key: 'gender' },
  { title: 'วันเกิด', key: 'birthDate' },
  { title: 'เลขบัตร', key: 'cardId' },
  { title: 'โทรศัพท์', key: 'telephone' },
  { title: 'จัดการ', key: 'actions', sortable: false },
]

const describeError = (error, fallback) => {
  const errors = error?.data?.errors
  if (errors && typeof errors === 'object')
    return Object.values(errors).flat().join(' ')

  return error?.data?.detail || error?.data?.title || fallback
}

const textOrNull = value => {
  if (value === null || value === undefined)
    return null

  const text = String(value).trim()

  return text === '' ? null : text
}

const intOrNull = value => {
  if (value === '' || value === null || value === undefined)
    return null

  const number = Number(value)

  return Number.isFinite(number) ? number : null
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
  foreName: item?.foreName ?? '',
  surname: item?.surname ?? '',
  gender: item?.gender ?? null,
  birthDate: item?.birthDate ? String(item.birthDate).slice(0, 10) : '',
  cardId: item?.cardId ?? '',
  telephone: item?.telephone ?? '',
  timeContact: item?.timeContact ?? '',
  addressType: item?.addressType ?? '',
  addressNo: item?.addressNo ?? '',
  road: item?.road ?? '',
  provinceId: item?.provinceId || null,
  districtId: item?.districtId || null,
  subDistrictId: item?.city || null,
  zipCode: item?.zipCode ?? '',
  mainClaim: item?.mainClaim ?? '',
  isActive: item?.isActive ?? true,
  education: item?.education ?? '',
  occupation: item?.occupation ?? '',
  isAllergy: item?.isAllergy ?? false,
  drugAllergy: item?.drugAllergy ?? '',
  isSmoke: item?.isSmoke ?? false,
  smoke: item?.smoke || null,
  smokeYear: item?.smokeYear ?? '',
  smokeCigarette: item?.smokeCigarette ?? '',
  cigaretteType: item?.cigaretteType || null,
  smokingQuit: item?.smokingQuit ?? false,
  smokingRemark: item?.smokingRemark ?? '',
  alcohol: item?.alcohol || null,
  alcoholFQ: item?.alcoholFQ ?? '',
})

const openAddDialog = async () => {
  await Promise.all([loadProvinces(), loadLifestyleOptions()])
  isEditMode.value = false
  selectedItem.value = null
  form.value = emptyForm()
  districts.value = []
  subDistricts.value = []
  saveError.value = ''
  birthDateError.value = ''
  isAddEditDialogVisible.value = true
}

const openEditDialog = async item => {
  await Promise.all([loadProvinces(), loadLifestyleOptions()])
  isEditMode.value = true
  selectedItem.value = item
  form.value = toForm(item)
  await prepareAddressLookups()
  saveError.value = ''
  birthDateError.value = ''
  isAddEditDialogVisible.value = true
}

const openViewDialog = item => {
  viewItem.value = item
  isViewDialogVisible.value = true
}

const buildBody = () => ({
  foreName: textOrNull(form.value.foreName),
  surname: textOrNull(form.value.surname),
  gender: form.value.gender || null,
  birthDate: form.value.birthDate || null,
  cardId: textOrNull(form.value.cardId),
  telephone: textOrNull(form.value.telephone),
  timeContact: textOrNull(form.value.timeContact),
  addressType: textOrNull(form.value.addressType),
  addressNo: textOrNull(form.value.addressNo),
  road: textOrNull(form.value.road),
  provinceId: textOrNull(form.value.provinceId),
  districtId: textOrNull(form.value.districtId),
  city: textOrNull(form.value.subDistrictId),
  zipCode: textOrNull(form.value.zipCode),
  mainClaim: textOrNull(form.value.mainClaim),
  isActive: form.value.isActive,
  education: textOrNull(form.value.education),
  occupation: textOrNull(form.value.occupation),
  isAllergy: form.value.isAllergy,
  drugAllergy: textOrNull(form.value.drugAllergy),
  isSmoke: form.value.isSmoke,
  smoke: form.value.smoke || null,
  smokeYear: intOrNull(form.value.smokeYear),
  smokeCigarette: intOrNull(form.value.smokeCigarette),
  cigaretteType: form.value.cigaretteType || null,
  smokingQuit: form.value.smokingQuit,
  smokingRemark: textOrNull(form.value.smokingRemark),
  alcohol: form.value.alcohol || null,
  alcoholFQ: intOrNull(form.value.alcoholFQ),
})

const saveItem = async () => {
  const validation = await refForm.value?.validate()
  if (!validation?.valid || birthDateError.value)
    return

  isSaving.value = true
  saveError.value = ''

  try {
    if (isEditMode.value) {
      await $api(`/patients/${selectedItem.value.id}`, {
        method: 'PUT',
        body: { id: selectedItem.value.id, ...buildBody() },
      })
    }
    else {
      await $api('/patients', {
        method: 'POST',
        body: buildBody(),
      })
    }

    isAddEditDialogVisible.value = false
    await fetchItems()
  } catch (error) {
    saveError.value = describeError(error, 'เกิดข้อผิดพลาดในการบันทึกข้อมูล')
  } finally {
    isSaving.value = false
  }
}

const display = value => (value === null || value === undefined || value === '' ? '-' : String(value))

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
        <template #item.birthDate="{ item }">
          {{ display(formatThaiDate(item.birthDate)) }}
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
        max-width="980"
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
                    v-model="form.foreName"
                    label="ชื่อ"
                    :rules="[requiredValidator]"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppTextField
                    v-model="form.surname"
                    label="นามสกุล"
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
                    clearable
                  />
                </VCol>
                <VCol cols="12" md="4">
                  <AppDateTimePicker
                    v-model="form.birthDate"
                    label="วันเกิด"
                    :placeholder="THAI_DATE_PLACEHOLDER"
                    :config="birthDatePickerConfig"
                    :error-messages="birthDateError"
                    clearable
                  />                </VCol>              
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppTextField
                    v-model="form.cardId"
                    label="เลขบัตร"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppTextField
                    v-model="form.telephone"
                    label="โทรศัพท์"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppTextField
                    v-model="form.timeContact"
                    label="เวลาที่ติดต่อได้"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppTextField
                    v-model="form.addressType"
                    label="ประเภทที่อยู่"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="8"
                >
                  <AppTextField
                    v-model="form.addressNo"
                    label="ที่อยู่"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppTextField
                    v-model="form.road"
                    label="ถนน"
                  />
                </VCol>

              
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppSelect
                    v-model="form.provinceId"
                    :items="provinces"
                    label="จังหวัด"
                    clearable
                    :loading="isProvincesLoading"
                    @update:model-value="onProvinceChange"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppSelect
                    v-model="form.districtId"
                    :items="districts"
                    label="อำเภอ"
                    clearable
                    :loading="isDistrictsLoading"
                    :disabled="!form.provinceId"
                    @update:model-value="onDistrictChange"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppSelect
                    v-model="form.subDistrictId"
                    :items="subDistricts"
                    label="ตำบล"
                    clearable
                    :loading="isSubDistrictsLoading"
                    :disabled="!form.districtId"
                    @update:model-value="onSubDistrictChange"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppTextField
                    v-model="form.zipCode"
                    label="รหัสไปรษณีย์"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppTextField
                    v-model="form.mainClaim"
                    label="สิทธิหลัก"
                  />
                </VCol>
           
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppTextField
                    v-model="form.education"
                    label="การศึกษา"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppTextField
                    v-model="form.occupation"
                    label="อาชีพ"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="6"
                >
                  <VCheckbox
                    v-model="form.isAllergy"
                    label="มีประวัติแพ้"
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
                  md="3"
                >
                  <VCheckbox
                    v-model="form.isSmoke"
                    label="สูบบุหรี่"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="3"
                >
                  <VCheckbox
                    v-model="form.smokingQuit"
                    label="เลิกบุหรี่แล้ว"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="3"
                >
                  <AppSelect
                    v-model="form.smoke"
                    :items="lifestyleOptions['smoking']"
                    label="การสูบ"
                    clearable
                    :loading="isLifestyleOptionsLoading"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="3"
                >
                  <AppTextField
                    v-model="form.smokeYear"
                    label="จำนวนปีที่สูบ"
                    type="number"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppTextField
                    v-model="form.smokeCigarette"
                    label="จำนวนมวน"
                    type="number"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppSelect
                    v-model="form.cigaretteType"
                    :items="lifestyleOptions['cigarette-type']"
                    label="ชนิดบุหรี่"
                    clearable
                    :loading="isLifestyleOptionsLoading"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="4"
                >
                  <AppTextField
                    v-model="form.smokingRemark"
                    label="หมายเหตุการสูบ"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="6"
                >
                  <AppSelect
                    v-model="form.alcohol"
                    :items="lifestyleOptions['drinking']"
                    label="แอลกอฮอล์"
                    clearable
                    :loading="isLifestyleOptionsLoading"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="6"
                >
                  <AppTextField
                    v-model="form.alcoholFQ"
                    label="ความถี่แอลกอฮอล์"
                    type="number"
                    min="0"
                    suffix="ครั้ง/สัปดาห์"
                  />
                </VCol>
                <VCol
                  cols="12"
                  md="4"
                  class="d-flex align-center"
                >
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
        max-width="720"
      >
        <VCard class="pa-2 pa-sm-8">
          <DialogCloseBtn @click="isViewDialogVisible = false" />
          <VCardText>
            <h4 class="text-h4 text-center mb-6">
              รายละเอียดผู้รับบริการ
            </h4>
            <VList v-if="viewItem">
              <VListItem title="รหัส" :subtitle="display(viewItem.id)" />
              <VListItem title="ชื่อ-สกุล" :subtitle="display(viewItem.fullName)" />
              <VListItem title="เพศ" :subtitle="display(viewItem.gender)" />
              <VListItem title="วันเกิด" :subtitle="display(formatThaiDate(viewItem.birthDate))" />
              <VListItem title="เลขบัตร" :subtitle="display(viewItem.cardId)" />
              <VListItem title="โทรศัพท์" :subtitle="display(viewItem.telephone)" />
              <VListItem title="ที่อยู่" :subtitle="display(viewItem.addressNo)" />
              <VListItem title="จังหวัด" :subtitle="display(viewItem.provinceName || viewItem.province?.name)" />
              <VListItem title="ใช้งาน" :subtitle="viewItem.isActive ? 'ใช่' : 'ไม่'" />
              <VListItem title="แพ้ยา" :subtitle="display(viewItem.drugAllergy)" />
              <VListItem title="อาชีพ" :subtitle="display(viewItem.occupation)" />
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
