<script setup>
import { $api } from '@/utils/api'
import { toBuddhistYear } from '@/utils/dateUtils'

// eslint-disable-next-line import/extensions
import moment from 'moment/min/moment-with-locales.js'
import { onMounted, ref } from 'vue'

moment.locale('th')

const route = useRoute()
const router = useRouter()

const id = Number(route.params.id)
const pharmacy = ref(null)
const errorMessage = ref(null)
const isConfirmDialogVisible = ref(false)
const progressDialogRef = ref(null)
const failureData = ref({})

const sections = [
  {
    title: 'ข้อมูลร้านขายยา',
    fields: [
      ['รหัสร้านยา', 'code'],
      ['ชื่อร้านยา', 'name'],
      ['ชื่อร้านยา (ชื่อที่ 2)', 'name2'],
      ['เลขที่ใบอนุญาต', 'licenseNo'],
      ['รหัส สปสช.', 'nhsoCode'],
      ['กลุ่มร้านยา', 'pharmacyGroupName'],
      ['ประเภทร้านยา', 'pharmacyTypeName'],
      ['ประเภทร้านยา (ระบุ)', 'pharmacyTypeOther'],
      ['ปีที่ขึ้นทะเบียน', 'regisYear'],
    ],
  },
  {
    title: 'ที่ตั้ง',
    fields: [
      ['เลขที่ / ที่อยู่', 'addressNo'],
      ['จังหวัด', 'provinceName'],
      ['อำเภอ', 'districtName'],
      ['ตำบล', 'subDistrictName'],
      ['รหัสไปรษณีย์', 'zipCode'],
      ['จังหวัดตาม อย.', 'fda_Province'],
      ['ละติจูด', 'lat'],
      ['ลองจิจูด', 'lng'],
    ],
  },
  {
    title: 'ช่องทางติดต่อ',
    fields: [
      ['เบอร์โทรสำนักงาน', 'office_Tel'],
      ['เบอร์แฟกซ์', 'office_Fax'],
      ['อีเมลสำนักงาน', 'office_Mail'],
      ['Line ID', 'lineID'],
      ['ชื่อผู้ประสานงาน', 'co_Name'],
      ['เบอร์โทรผู้ประสานงาน', 'co_Tel'],
      ['อีเมลผู้ประสานงาน', 'co_Mail'],
    ],
  },
]

const load = async () => {
  try {
    pharmacy.value = await $api(`/pharmacies/${id}`, { method: 'GET' })
  } catch {
    errorMessage.value = 'ไม่พบข้อมูลร้านขายยา'
  }
}

onMounted(load)

const deactivate = async () => {
  progressDialogRef.value?.startProgress()
  try {
    await $api(`/pharmacies/${id}`, {
      method: 'DELETE',
      query: { rowVersion: pharmacy.value.rowVersion },
    })
    progressDialogRef.value?.stopProgress()
    progressDialogRef.value?.showSuccess()
    await load()
  } catch (error) {
    failureData.value = { message: [error?.data?.detail ?? 'ปิดการใช้งานร้านขายยาไม่สำเร็จ'] }
    progressDialogRef.value?.stopProgress()
    progressDialogRef.value?.showFailure()
  }
}
</script>

<template>
  <VCard>
    <VCardItem>
      <VCardTitle>รายละเอียดร้านขายยา</VCardTitle>
      <template #append>
        <div
          v-if="pharmacy"
          class="d-flex gap-2"
        >
          <VBtn
            variant="tonal"
            color="secondary"
            @click="router.push({ name: 'pharmacies-list' })"
          >
            กลับ
          </VBtn>
          <VBtn
            prepend-icon="tabler-pencil"
            data-testid="pharmacies-edit-button"
            @click="router.push({ name: 'pharmacies-edit-id', params: { id } })"
          >
            แก้ไข
          </VBtn>
          <VBtn
            v-if="pharmacy.isActive"
            color="error"
            variant="tonal"
            prepend-icon="tabler-trash"
            data-testid="pharmacies-deactivate-button"
            @click="isConfirmDialogVisible = true"
          >
            ปิดการใช้งาน
          </VBtn>
        </div>
      </template>
    </VCardItem>

    <VCardText>
      <VAlert
        v-if="errorMessage"
        type="error"
        variant="tonal"
      >
        {{ errorMessage }}
      </VAlert>

      <template v-if="pharmacy">
        <VChip
          :color="pharmacy.isActive ? 'success' : 'error'"
          size="small"
          label
          class="mb-4"
        >
          {{ pharmacy.isActive ? 'ใช้งาน' : 'ไม่ใช้งาน' }}
        </VChip>

        <div
          v-for="section in sections"
          :key="section.title"
          class="mb-6"
        >
          <h6 class="text-h6 mb-2">
            {{ section.title }}
          </h6>
          <VRow>
            <VCol
              v-for="[label, key] in section.fields"
              :key="key"
              cols="12"
              md="4"
            >
              <div class="text-sm text-medium-emphasis">
                {{ label }}
              </div>
              <div class="text-body-1">
                {{ pharmacy[key] || '-' }}
              </div>
            </VCol>
          </VRow>
        </div>

        <div class="text-sm text-medium-emphasis">
          ปรับปรุงล่าสุดเมื่อ
          {{ pharmacy.lastModified ? toBuddhistYear(moment(pharmacy.lastModified), 'LLL') : '-' }}
        </div>
      </template>
    </VCardText>

    <ConfirmProgressDialog
      ref="progressDialogRef"
      v-model:model-value="isConfirmDialogVisible"
      confirm-title="ยืนยันการปิดการใช้งานร้านขายยา"
      confirm-message="ร้านจะไม่แสดงในรายการ แต่ข้อมูลยังถูกเก็บไว้"
      progress-message="กำลังประมวลผล, โปรดรอสักครู่..."
      success-title="ปิดการใช้งานสำเร็จ"
      success-message="ปิดการใช้งานร้านขายยาแล้ว"
      failure-title="ปิดการใช้งานไม่สำเร็จ"
      failure-message="โปรดตรวจสอบแล้วลองอีกครั้ง"
      :failure-data="failureData"
      @confirm="deactivate"
      @retry="deactivate"
    />
  </VCard>
</template>
