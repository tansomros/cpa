<script setup>
import { requiredValidator } from '@/@core/utils/validators'
import { $api } from '@/utils/api'
import { computed, onMounted, ref, watch } from 'vue'

const props = defineProps({
  showStatus: { type: Boolean, default: false },
  saving: { type: Boolean, default: false },
  serverErrors: { type: Object, default: () => ({}) },
})

const emit = defineEmits(['submit', 'cancel'])

const form = defineModel({ type: Object, required: true })

const OTHER_PHARMACY_TYPE_CODE = 'อื่นๆ'

const refForm = ref()

const groups = ref([])
const types = ref([])
const provinces = ref([])
const districts = ref([])
const subDistricts = ref([])

const toNumberOptions = options => options.map(o => ({ value: Number(o.value), title: o.title }))

const loadLookups = async (provinceId, districtId) => {
  const query = {}
  if (provinceId) query.provinceId = provinceId
  if (districtId) query.districtId = districtId

  return await $api('/pharmacies/lookups', { method: 'GET', query })
}

onMounted(async () => {
  const lookups = await loadLookups(form.value.provinceId, form.value.districtId)

  groups.value = toNumberOptions(lookups.groups ?? [])
  types.value = toNumberOptions(lookups.types ?? [])
  provinces.value = lookups.provinces ?? []
  districts.value = lookups.districts ?? []
  subDistricts.value = lookups.subDistricts ?? []
})

const onProvinceChange = async provinceId => {
  form.value.provinceId = provinceId
  form.value.districtId = null
  form.value.subDistrictId = null
  subDistricts.value = []
  districts.value = provinceId ? (await loadLookups(provinceId)).districts ?? [] : []
}

const onDistrictChange = async districtId => {
  form.value.districtId = districtId
  form.value.subDistrictId = null
  subDistricts.value = districtId
    ? (await loadLookups(form.value.provinceId, districtId)).subDistricts ?? []
    : []
}

const onSubDistrictChange = subDistrictId => {
  form.value.subDistrictId = subDistrictId

  const match = subDistricts.value.find(s => s.value === subDistrictId)?.title.match(/\((\d{5})\)$/)
  if (match && !form.value.zipCode)
    form.value.zipCode = match[1]
}

const isOtherType = computed(() =>
  types.value.find(t => t.value === form.value.pharmacyTypeId)?.title === OTHER_PHARMACY_TYPE_CODE)

watch(isOtherType, value => {
  if (!value)
    form.value.pharmacyTypeOther = null
})

const fieldErrors = field => {
  const key = Object.keys(props.serverErrors).find(k => k.toLowerCase() === field.toLowerCase())

  return key ? props.serverErrors[key] : []
}

const submit = async () => {
  const { valid } = await refForm.value.validate()
  if (valid)
    emit('submit')
}
</script>

<template>
  <VForm
    ref="refForm"
    @submit.prevent="submit"
  >
    <VRow>
      <VCol cols="12">
        <h6 class="text-h6">
          ข้อมูลร้านขายยา
        </h6>
      </VCol>
      <VCol
        cols="12"
        md="4"
      >
        <AppTextField
          v-model="form.code"
          label="รหัสร้านยา *"
          :rules="[requiredValidator]"
          :error-messages="fieldErrors('code')"
          data-testid="pharmacies-code-input"
        />
      </VCol>
      <VCol
        cols="12"
        md="4"
      >
        <AppTextField
          v-model="form.licenseNo"
          label="เลขที่ใบอนุญาต"
        />
      </VCol>
      <VCol
        cols="12"
        md="4"
      >
        <AppTextField
          v-model="form.nhsoCode"
          label="รหัส สปสช."
        />
      </VCol>
      <VCol
        cols="12"
        md="6"
      >
        <AppTextField
          v-model="form.name"
          label="ชื่อร้านยา *"
          :rules="[requiredValidator]"
          :error-messages="fieldErrors('name')"
          data-testid="pharmacies-name-input"
        />
      </VCol>
      <VCol
        cols="12"
        md="6"
      >
        <AppTextField
          v-model="form.name2"
          label="ชื่อร้านยา (ชื่อที่ 2)"
        />
      </VCol>
      <VCol
        cols="12"
        md="4"
      >
        <AppSelect
          v-model="form.pharmacyGroupId"
          :items="groups"
          label="กลุ่มร้านยา"
          clearable
          :error-messages="fieldErrors('pharmacyGroupId')"
        />
      </VCol>
      <VCol
        cols="12"
        md="4"
      >
        <AppSelect
          v-model="form.pharmacyTypeId"
          :items="types"
          label="ประเภทร้านยา"
          clearable
          :error-messages="fieldErrors('pharmacyTypeId')"
        />
      </VCol>
      <VCol
        v-if="isOtherType"
        cols="12"
        md="4"
      >
        <AppTextField
          v-model="form.pharmacyTypeOther"
          label="ประเภทร้านยา (ระบุ)"
        />
      </VCol>
      <VCol
        cols="12"
        md="4"
      >
        <AppTextField
          v-model="form.regisYear"
          label="ปีที่ขึ้นทะเบียน"
        />
      </VCol>

      <VCol cols="12">
        <VDivider class="my-2" />
        <h6 class="text-h6">
          ที่ตั้ง
        </h6>
      </VCol>
      <VCol cols="12">
        <AppTextField
          v-model="form.addressNo"
          label="เลขที่ / ที่อยู่"
        />
      </VCol>
      <VCol
        cols="12"
        md="4"
      >
        <AppAutocomplete
          :model-value="form.provinceId"
          :items="provinces"
          label="จังหวัด"
          clearable
          :error-messages="fieldErrors('provinceId')"
          @update:model-value="onProvinceChange"
        />
      </VCol>
      <VCol
        cols="12"
        md="4"
      >
        <AppAutocomplete
          :model-value="form.districtId"
          :items="districts"
          label="อำเภอ"
          clearable
          :disabled="!form.provinceId"
          :error-messages="fieldErrors('districtId')"
          @update:model-value="onDistrictChange"
        />
      </VCol>
      <VCol
        cols="12"
        md="4"
      >
        <AppAutocomplete
          :model-value="form.subDistrictId"
          :items="subDistricts"
          label="ตำบล"
          clearable
          :disabled="!form.districtId"
          :error-messages="fieldErrors('subDistrictId')"
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
          v-model="form.fda_Province"
          label="จังหวัดตาม อย."
        />
      </VCol>
      <VCol
        cols="12"
        md="2"
      >
        <AppTextField
          v-model="form.lat"
          label="ละติจูด"
        />
      </VCol>
      <VCol
        cols="12"
        md="2"
      >
        <AppTextField
          v-model="form.lng"
          label="ลองจิจูด"
        />
      </VCol>

      <VCol cols="12">
        <VDivider class="my-2" />
        <h6 class="text-h6">
          ช่องทางติดต่อ
        </h6>
      </VCol>
      <VCol
        cols="12"
        md="4"
      >
        <AppTextField
          v-model="form.office_Tel"
          label="เบอร์โทรสำนักงาน"
        />
      </VCol>
      <VCol
        cols="12"
        md="4"
      >
        <AppTextField
          v-model="form.office_Fax"
          label="เบอร์แฟกซ์"
        />
      </VCol>
      <VCol
        cols="12"
        md="4"
      >
        <AppTextField
          v-model="form.office_Mail"
          label="อีเมลสำนักงาน"
        />
      </VCol>
      <VCol
        cols="12"
        md="4"
      >
        <AppTextField
          v-model="form.lineID"
          label="Line ID"
        />
      </VCol>
      <VCol
        cols="12"
        md="4"
      >
        <AppTextField
          v-model="form.co_Name"
          label="ชื่อผู้ประสานงาน"
        />
      </VCol>
      <VCol
        cols="12"
        md="4"
      >
        <AppTextField
          v-model="form.co_Tel"
          label="เบอร์โทรผู้ประสานงาน"
        />
      </VCol>
      <VCol
        cols="12"
        md="4"
      >
        <AppTextField
          v-model="form.co_Mail"
          label="อีเมลผู้ประสานงาน"
        />
      </VCol>

      <VCol
        v-if="props.showStatus"
        cols="12"
      >
        <VSwitch
          v-model="form.isActive"
          label="สถานะการใช้งาน"
          color="success"
          data-testid="pharmacies-status-switch"
        />
      </VCol>

      <VCol
        cols="12"
        class="d-flex justify-end gap-4"
      >
        <VBtn
          color="secondary"
          variant="tonal"
          @click="emit('cancel')"
        >
          ยกเลิก
        </VBtn>
        <VBtn
          type="submit"
          :loading="props.saving"
          data-testid="pharmacies-save-button"
        >
          บันทึก
        </VBtn>
      </VCol>
    </VRow>
  </VForm>
</template>
