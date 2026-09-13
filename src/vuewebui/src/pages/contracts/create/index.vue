<script setup>
import { requiredValidator } from '@/@core/utils/validators'
import { formatCurrency } from '@/utils/numberUtils'
import { ref, computed, watch } from 'vue'
import { useRouter } from 'vue-router'
import { VForm } from 'vuetify/components/VForm'

// eslint-disable-next-line import/extensions
import moment from 'moment/min/moment-with-locales.js'

moment.locale('th')

const router = useRouter()
const isProgressDialogVisible = ref(false)
const progressDialogRef = ref(null)
const progressDialogFailureDescription = ref(null)
const refForm = ref()
const createdContractId = ref(null)

// 👉 Stepper
const currentStep = ref(0)
const steps = [
  { title: 'ข้อมูลตั้งโครงการ', subtitle: 'รายละเอียดโครงการ', icon: 'tabler-clipboard-list' },
  { title: 'กำหนดราคากลาง', subtitle: 'แบบ บก.06', icon: 'tabler-calculator' },
  { title: 'คกก.ร่าง TOR', subtitle: 'แต่งตั้งคณะกรรมการ', icon: 'tabler-users-group' },
  { title: 'คกก.จัดซื้อ', subtitle: 'e-bidding', icon: 'tabler-shopping-cart' },
  { title: 'คกก.ตรวจรับ', subtitle: 'แต่งตั้งคณะกรรมการ', icon: 'tabler-checklist' },
  { title: 'ผลการ e-bidding', subtitle: 'ผู้ชนะการประกวดราคา', icon: 'tabler-trophy' },
  { title: 'ข้อมูลสัญญา', subtitle: 'รายละเอียดสัญญา', icon: 'tabler-file-dollar' },
  { title: 'ผลการตรวจรับ', subtitle: 'อนุมัติ/ไม่อนุมัติ', icon: 'tabler-clipboard-check' },
]

// 👉 Load reference data
const { data: divisionData } = await useApi(`/divisions/GetAllDivisionList`, { method: 'GET' })
const divisions = computed(() => {
  if (!divisionData.value?.divisions) return []
  return divisionData.value.divisions.map(d => ({
    title: d.name,
    value: d.id,
  }))
})

// Mock employee list — replace with real API
const { data: employeeData } = await useApi(`/employees`, { method: 'GET' })
const employees = computed(() => {
  if (!employeeData.value) return []
  const list = Array.isArray(employeeData.value) ? employeeData.value : (employeeData.value.employees || [])
  return list.map(e => ({
    title: e.fullName || e.name || `${e.firstName} ${e.lastName}`,
    value: e.id,
  }))
})

// Mock company list — replace with real API
const { data: companyData } = await useApi(`/companies`, { method: 'GET' })
const companies = computed(() => {
  if (!companyData.value) return []
  const list = Array.isArray(companyData.value) ? companyData.value : (companyData.value.companies || [])
  return list.map(c => ({
    title: c.name || c.companyName,
    value: c.id,
  }))
})

// Mock department list
const { data: departmentData } = await useApi(`/departments`, { method: 'GET' })
const departments = computed(() => {
  if (!departmentData.value) return []
  const list = Array.isArray(departmentData.value) ? departmentData.value : (departmentData.value.departments || [])
  return list.map(d => ({
    title: d.name,
    value: d.id,
  }))
})

// 👉 Unit options
const unitOptions = [
  'ชิ้น', 'ชุด', 'เครื่อง', 'ตัว', 'อัน', 'แผ่น', 'กล่อง', 'หลอด', 'ขวด',
  'ม้วน', 'ถุง', 'กก.', 'งาน', 'ครั้ง', 'เดือน', 'ปี', 'รายการ', 'โครงการ',
]

// 👉 Contract type options
const contractTypeOptions = [
  'สัญญาซื้อขาย',
  'สัญญาจ้าง',
  'สัญญาเช่า',
  'สัญญาจ้างที่ปรึกษา',
  'สัญญาจ้างออกแบบ',
  'ใบสั่งซื้อ',
  'ใบสั่งจ้าง',
  'อื่นๆ',
]

// 👉 Guarantee type options
const guaranteeTypeOptions = [
  'หนังสือค้ำประกันธนาคาร',
  'เช็คที่ธนาคารสั่งจ่าย',
  'พันธบัตรรัฐบาลไทย',
  'เงินสด',
  'อื่นๆ',
]

// 👉 Price method options
const priceMethodOptions = [
  'สืบราคาจากท้องตลาด',
  'ราคาที่เคยซื้อครั้งหลังสุดภายใน 2 ปี',
  'ราคามาตรฐานที่สำนักงบประมาณกำหนด',
  'ราคาตามท้องตลาด',
  'อื่นๆ',
]

// ═══════════════════════════════════════════════
// 👉 SECTION 1: ข้อมูลตั้งโครงการ
// ═══════════════════════════════════════════════
const form = ref({
  // Section 1
  referenceDocument: null,
  itemDescription: null,
  quantity: null,
  unit: null,
  unitPrice: null,
  totalAmountText: null,
  sectorId: null,
  departmentId: null,

  // Section 2
  deliveryDue: null,
  priceMethod: null,
  priceInquiryCompanies: [{ companyId: null }],
  centralPricePerUnit: null,

  // Section 3
  torChairman: null,
  torCommitteeMembers: [{ employeeId: null }],
  torSecretary: null,
  torOrderNumber: null,
  torOrderDate: null,

  // Section 4
  purchaseChairman: null,
  purchaseCommitteeMembers: [{ employeeId: null }],
  purchaseOrderNumber: null,
  purchaseOrderDate: null,

  // Section 5
  inspectionChairman: null,
  inspectionCommitteeMembers: [{ employeeId: null }],
  inspectionSecretary: null,
  inspectionOrderNumber: null,
  inspectionOrderDate: null,

  // Section 6
  bidWinnerId: null,
  priceReduction: null,
  lowestPrice: null,
  biddingRemark: null,

  // Section 7
  contractType: null,
  contractNumber: null,
  contractSignDate: null,
  contractQuantity: null,
  contractUnit: null,
  contractUnitPrice: null,
  contractTotalPrice: null,
  contractInstallments: null,
  contractStartDate: null,
  contractEndDate: null,
  guaranteeType: null,
  guaranteeReference: null,
  guaranteeAmount: null,
  guaranteePeriod: null,

  // Section 8
  inspectionDate: null,
  inspectionResult: null,
  inspectionRemark: null,
  location: null,
})

// 👉 Computed: Total Amount
const totalAmount = computed(() => {
  const qty = parseFloat(form.value.quantity) || 0
  const price = parseFloat(form.value.unitPrice) || 0
  return qty * price
})

// 👉 Computed: Central Price Total
const centralPriceTotal = computed(() => {
  const qty = parseFloat(form.value.quantity) || 0
  const price = parseFloat(form.value.centralPricePerUnit) || 0
  return qty * price
})

// 👉 Computed: bid winner companies (from section 2 selected companies)
const bidWinnerCompanies = computed(() => {
  return form.value.priceInquiryCompanies
    .filter(c => c.companyId)
    .map(c => {
      const found = companies.value.find(co => co.value === c.companyId)
      return found || { title: c.companyId, value: c.companyId }
    })
})

// 👉 Dynamic arrays: add/remove
const addPriceInquiryCompany = () => {
  form.value.priceInquiryCompanies.push({ companyId: null })
}
const removePriceInquiryCompany = index => {
  if (form.value.priceInquiryCompanies.length > 1) {
    form.value.priceInquiryCompanies.splice(index, 1)
  }
}

const addTorCommittee = () => {
  form.value.torCommitteeMembers.push({ employeeId: null })
}
const removeTorCommittee = index => {
  if (form.value.torCommitteeMembers.length > 1) {
    form.value.torCommitteeMembers.splice(index, 1)
  }
}

const addPurchaseCommittee = () => {
  form.value.purchaseCommitteeMembers.push({ employeeId: null })
}
const removePurchaseCommittee = index => {
  if (form.value.purchaseCommitteeMembers.length > 1) {
    form.value.purchaseCommitteeMembers.splice(index, 1)
  }
}

const addInspectionCommittee = () => {
  form.value.inspectionCommitteeMembers.push({ employeeId: null })
}
const removeInspectionCommittee = index => {
  if (form.value.inspectionCommitteeMembers.length > 1) {
    form.value.inspectionCommitteeMembers.splice(index, 1)
  }
}

// 👉 Step navigation
const nextStep = () => {
  if (currentStep.value < steps.length - 1) {
    currentStep.value++
  }
}
const prevStep = () => {
  if (currentStep.value > 0) {
    currentStep.value--
  }
}

// 👉 Submit
const create = async () => {
  if (!refForm.value) return
  const isValid = await refForm.value.validate()
  if (!isValid.valid) return

  isProgressDialogVisible.value = true
  progressDialogRef.value.startProgress()

  const contractData = {
    ...form.value,
    totalAmount: totalAmount.value,
    centralPriceTotal: centralPriceTotal.value,
  }

  try {
    const response = await $api(`/contracts`, { method: 'POST', body: contractData })
    if (response) {
      createdContractId.value = response.id || response
    }
    progressDialogRef.value.stopProgress()
    progressDialogRef.value.showSuccess()
  } catch (error) {
    if (error.data) {
      progressDialogFailureDescription.value = error.data?.errors || [{ error: 'ขออภัย, ระบบเกิดข้อผิดพลาด.' }]
    } else if (error.request) {
      progressDialogFailureDescription.value = 'ขออภัย, ไม่สามารถให้บริการได้ในขณะนี้, โปรดลองใหม่อีกครั้ง'
    } else {
      progressDialogFailureDescription.value = error.message
    }
    progressDialogRef.value.stopProgress()
    progressDialogRef.value.showFailure()
  }
}

const cancelCreate = () => {
  progressDialogRef.value.closeDialog()
  isProgressDialogVisible.value = false
}

const createSuccess = () => {
  isProgressDialogVisible.value = false
  router.push(`/contracts/view/${createdContractId.value}`)
}

// 👉 Inspection result options
const inspectionResultOptions = [
  { title: 'อนุมัติ', value: 'approved' },
  { title: 'ไม่อนุมัติ', value: 'rejected' },
]
</script>

<template>
  <!-- Breadcrumb -->
  <div class="d-flex flex-column mb-6">
    <div class="text-body-2 d-flex align-center gap-1 mt-1">
      <IconBtn><VIcon icon="tabler-home" /></IconBtn>
      <RouterLink to="/" class="text-primary">หน้าหลัก</RouterLink>
      <span>>></span>
      <RouterLink to="/contracts/list" class="text-primary">รายการข้อมูลสัญญา</RouterLink>
      <span>>></span>
      <span class="text-medium-emphasis">เพิ่มข้อมูลสัญญา</span>
    </div>
  </div>

  <div>
    <VForm
      ref="refForm"
      @submit.prevent
    >
      <!-- Header Card -->
      <VRow>
        <VCol cols="12">
          <VCard class="mb-6">
            <VCardItem>
              <div class="d-flex flex-wrap justify-start justify-sm-space-between gap-y-4 gap-x-6">
                <div class="d-flex flex-column justify-center">
                  <VCardTitle class="text-h5">
                    <VIcon icon="tabler-file-plus" class="me-2" />
                    เพิ่มข้อมูลสัญญา
                  </VCardTitle>
                  <VCardSubtitle>กรอกข้อมูลรายละเอียดสัญญาตามขั้นตอน</VCardSubtitle>
                </div>

                <div class="d-flex gap-4 align-center flex-wrap">
                  <VBtn
                    variant="tonal"
                    color="secondary"
                    prepend-icon="tabler-arrow-left"
                    :to="{ name: 'contracts-list' }"
                  >
                    กลับรายการ
                  </VBtn>
                  <VBtn
                    type="submit"
                    color="primary"
                    prepend-icon="tabler-device-floppy"
                    :disabled="isProgressDialogVisible"
                    @click="create"
                  >
                    บันทึก
                  </VBtn>
                </div>
              </div>
            </VCardItem>
          </VCard>
        </VCol>
      </VRow>

      <VRow>
        <!-- 👉 Left Sidebar: Step Navigation -->
        <VCol
          cols="12"
          md="3"
        >
          <VCard class="stepper-sidebar">
            <VCardText class="pa-4">
              <div
                v-for="(step, index) in steps"
                :key="index"
                class="stepper-item d-flex align-center gap-3 pa-3 rounded-lg cursor-pointer"
                :class="{
                  'stepper-item--active': currentStep === index,
                  'stepper-item--completed': currentStep > index,
                  'mb-2': index < steps.length - 1,
                }"
                @click="currentStep = index"
              >
                <VAvatar
                  :color="currentStep === index ? 'primary' : currentStep > index ? 'success' : 'default'"
                  :variant="currentStep === index ? 'elevated' : 'tonal'"
                  size="40"
                  rounded
                >
                  <VIcon
                    v-if="currentStep > index"
                    icon="tabler-check"
                    size="20"
                  />
                  <VIcon
                    v-else
                    :icon="step.icon"
                    size="20"
                  />
                </VAvatar>

                <div class="d-flex flex-column">
                  <span
                    class="text-body-1 font-weight-medium"
                    :class="{ 'text-primary': currentStep === index }"
                  >
                    {{ step.title }}
                  </span>
                  <span class="text-body-2 text-disabled">{{ step.subtitle }}</span>
                </div>
              </div>
            </VCardText>
          </VCard>
        </VCol>

        <!-- 👉 Right Content: Form Sections -->
        <VCol
          cols="12"
          md="9"
        >
          <!-- ═══════════════════════════════════════════════ -->
          <!-- STEP 1: ข้อมูลตั้งโครงการ -->
          <!-- ═══════════════════════════════════════════════ -->
          <VCard
            v-show="currentStep === 0"
            class="mb-6"
          >
            <VCardItem>
              <VCardTitle>
                <VIcon icon="tabler-clipboard-list" class="me-2" color="primary" />
                ข้อมูลตั้งโครงการ
              </VCardTitle>
              <VCardSubtitle>กรอกรายละเอียดข้อมูลโครงการ</VCardSubtitle>
            </VCardItem>

            <VDivider />

            <VCardText>
              <VRow>
                <VCol cols="12">
                  <AppTextField
                    v-model="form.referenceDocument"
                    label="อ้างอิงเอกสารคำขอ"
                    placeholder="ระบุเลขที่เอกสารคำขอ"
                  />
                </VCol>

                <VCol cols="12">
                  <AppTextField
                    v-model="form.itemDescription"
                    label="รายการและรายละเอียดประกอบ"
                    placeholder="ระบุรายการและรายละเอียด"
                  />
                </VCol>

                <VCol cols="12" md="4">
                  <AppTextField
                    v-model="form.quantity"
                    label="จำนวน"
                    placeholder="ระบุจำนวน"
                    type="number"
                  />
                </VCol>

                <VCol cols="12" md="4">
                  <AppSelect
                    v-model="form.unit"
                    label="หน่วยนับ"
                    placeholder="เลือกหน่วยนับ"
                    :items="unitOptions"
                  />
                </VCol>

                <VCol cols="12" md="4">
                  <AppTextField
                    v-model="form.unitPrice"
                    label="ราคาต่อหน่วย"
                    placeholder="0.00"
                    type="number"
                    prefix="฿"
                  />
                </VCol>

                <VCol cols="12" md="6">
                  <div class="pa-4 rounded-lg bg-light-primary">
                    <div class="text-body-2 text-disabled mb-1">รวมเงิน (บาท) ตัวเลข</div>
                    <div class="text-h5 font-weight-bold text-primary">
                      {{ formatCurrency(totalAmount) }}
                    </div>
                  </div>
                </VCol>

                <VCol cols="12" md="6">
                  <AppTextField
                    v-model="form.totalAmountText"
                    label="รวมเงิน (บาท) ตัวอักษร"
                    placeholder="ระบุจำนวนเงินเป็นตัวอักษร"
                  />
                </VCol>

                <VCol cols="12" md="6">
                  <AppSelect
                    v-model="form.sectorId"
                    label="ฝ่าย"
                    placeholder="เลือกฝ่าย"
                    :items="divisions"
                  />
                </VCol>

                <VCol cols="12" md="6">
                  <AppSelect
                    v-model="form.departmentId"
                    label="แผนก"
                    placeholder="เลือกแผนก"
                    :items="departments"
                  />
                </VCol>
              </VRow>
            </VCardText>
          </VCard>

          <!-- ═══════════════════════════════════════════════ -->
          <!-- STEP 2: กำหนดราคากลาง (แบบ บก.06) -->
          <!-- ═══════════════════════════════════════════════ -->
          <VCard
            v-show="currentStep === 1"
            class="mb-6"
          >
            <VCardItem>
              <VCardTitle>
                <VIcon icon="tabler-calculator" class="me-2" color="primary" />
                กำหนดราคากลาง (แบบ บก.06)
              </VCardTitle>
              <VCardSubtitle>ข้อมูลสืบราคาและกำหนดราคากลาง</VCardSubtitle>
            </VCardItem>

            <VDivider />

            <VCardText>
              <VRow>
                <VCol cols="12" md="6">
                  <AppTextField
                    v-model="form.deliveryDue"
                    label="กำหนดส่งมอบ"
                    placeholder="ระบุกำหนดส่งมอบ"
                  />
                </VCol>

                <VCol cols="12" md="6">
                  <AppSelect
                    v-model="form.priceMethod"
                    label="วิธีกำหนดราคากลาง"
                    placeholder="เลือกวิธีกำหนดราคากลาง"
                    :items="priceMethodOptions"
                  />
                </VCol>

                <!-- Dynamic: บริษัทสืบราคา -->
                <VCol cols="12">
                  <div class="d-flex align-center justify-space-between mb-3">
                    <label class="text-body-1 font-weight-medium">ชื่อบริษัทสืบราคา</label>
                    <VBtn
                      size="small"
                      variant="tonal"
                      color="primary"
                      prepend-icon="tabler-plus"
                      @click="addPriceInquiryCompany"
                    >
                      เพิ่มบริษัท
                    </VBtn>
                  </div>

                  <div
                    v-for="(company, index) in form.priceInquiryCompanies"
                    :key="index"
                    class="d-flex align-center gap-3 mb-3"
                  >
                    <span class="text-body-2 text-disabled" style="min-inline-size: 30px;">
                      {{ index + 1 }}.
                    </span>
                    <AppSelect
                      v-model="company.companyId"
                      placeholder="เลือกบริษัท"
                      :items="companies"
                      class="flex-grow-1"
                    />
                    <VBtn
                      icon
                      variant="text"
                      color="error"
                      size="small"
                      :disabled="form.priceInquiryCompanies.length <= 1"
                      @click="removePriceInquiryCompany(index)"
                    >
                      <VIcon icon="tabler-trash" size="18" />
                    </VBtn>
                  </div>
                </VCol>

                <VCol cols="12" md="6">
                  <AppTextField
                    v-model="form.centralPricePerUnit"
                    label="ราคากลางต่อหน่วย"
                    placeholder="0.00"
                    type="number"
                    prefix="฿"
                  />
                </VCol>

                <VCol cols="12" md="6">
                  <div class="pa-4 rounded-lg bg-light-primary">
                    <div class="text-body-2 text-disabled mb-1">ราคากลาง รวม</div>
                    <div class="text-h5 font-weight-bold text-primary">
                      {{ formatCurrency(centralPriceTotal) }}
                    </div>
                  </div>
                </VCol>
              </VRow>
            </VCardText>
          </VCard>

          <!-- ═══════════════════════════════════════════════ -->
          <!-- STEP 3: คณะกรรมการร่างขอบเขตของงาน (TOR) -->
          <!-- ═══════════════════════════════════════════════ -->
          <VCard
            v-show="currentStep === 2"
            class="mb-6"
          >
            <VCardItem>
              <VCardTitle>
                <VIcon icon="tabler-users-group" class="me-2" color="primary" />
                แต่งตั้ง คณะกรรมการร่างขอบเขตของงาน
              </VCardTitle>
              <VCardSubtitle>กำหนดคณะกรรมการ TOR</VCardSubtitle>
            </VCardItem>

            <VDivider />

            <VCardText>
              <VRow>
                <VCol cols="12" md="6">
                  <AppSelect
                    v-model="form.torChairman"
                    label="ผู้รับมอบหมาย/ประธาน TOR"
                    placeholder="เลือกพนักงาน"
                    :items="employees"
                  />
                </VCol>

                <VCol cols="12" md="6">
                  <AppSelect
                    v-model="form.torSecretary"
                    label="เลขานุการ กก. TOR"
                    placeholder="เลือกพนักงาน"
                    :items="employees"
                  />
                </VCol>

                <!-- Dynamic: กรรมการ TOR -->
                <VCol cols="12">
                  <div class="d-flex align-center justify-space-between mb-3">
                    <label class="text-body-1 font-weight-medium">กรรมการ TOR</label>
                    <VBtn
                      size="small"
                      variant="tonal"
                      color="primary"
                      prepend-icon="tabler-plus"
                      @click="addTorCommittee"
                    >
                      เพิ่มกรรมการ
                    </VBtn>
                  </div>

                  <div
                    v-for="(member, index) in form.torCommitteeMembers"
                    :key="index"
                    class="d-flex align-center gap-3 mb-3"
                  >
                    <span class="text-body-2 text-disabled" style="min-inline-size: 30px;">
                      {{ index + 1 }}.
                    </span>
                    <AppSelect
                      v-model="member.employeeId"
                      placeholder="เลือกพนักงาน"
                      :items="employees"
                      class="flex-grow-1"
                    />
                    <VBtn
                      icon
                      variant="text"
                      color="error"
                      size="small"
                      :disabled="form.torCommitteeMembers.length <= 1"
                      @click="removeTorCommittee(index)"
                    >
                      <VIcon icon="tabler-trash" size="18" />
                    </VBtn>
                  </div>
                </VCol>

                <VCol cols="12" md="6">
                  <AppTextField
                    v-model="form.torOrderNumber"
                    label="เลขที่คำสั่ง TOR"
                    placeholder="ระบุเลขที่คำสั่ง"
                  />
                </VCol>

                <VCol cols="12" md="6">
                  <AppDateTimePicker
                    v-model="form.torOrderDate"
                    label="วันที่คำสั่ง TOR"
                    placeholder="เลือกวันที่"
                  />
                </VCol>
              </VRow>
            </VCardText>
          </VCard>

          <!-- ═══════════════════════════════════════════════ -->
          <!-- STEP 4: คณะกรรมการจัดซื้อ (e-bidding) -->
          <!-- ═══════════════════════════════════════════════ -->
          <VCard
            v-show="currentStep === 3"
            class="mb-6"
          >
            <VCardItem>
              <VCardTitle>
                <VIcon icon="tabler-shopping-cart" class="me-2" color="primary" />
                แต่งตั้ง คณะกรรมการจัดซื้อ (e-bidding)
              </VCardTitle>
              <VCardSubtitle>กำหนดคณะกรรมการจัดซื้อ</VCardSubtitle>
            </VCardItem>

            <VDivider />

            <VCardText>
              <VRow>
                <VCol cols="12" md="6">
                  <AppSelect
                    v-model="form.purchaseChairman"
                    label="ประธานคณะกรรมการจัดซื้อ"
                    placeholder="เลือกพนักงาน"
                    :items="employees"
                  />
                </VCol>

                <!-- Dynamic: กรรมการจัดซื้อ -->
                <VCol cols="12">
                  <div class="d-flex align-center justify-space-between mb-3">
                    <label class="text-body-1 font-weight-medium">กรรมการจัดซื้อ</label>
                    <VBtn
                      size="small"
                      variant="tonal"
                      color="primary"
                      prepend-icon="tabler-plus"
                      @click="addPurchaseCommittee"
                    >
                      เพิ่มกรรมการ
                    </VBtn>
                  </div>

                  <div
                    v-for="(member, index) in form.purchaseCommitteeMembers"
                    :key="index"
                    class="d-flex align-center gap-3 mb-3"
                  >
                    <span class="text-body-2 text-disabled" style="min-inline-size: 30px;">
                      {{ index + 1 }}.
                    </span>
                    <AppSelect
                      v-model="member.employeeId"
                      placeholder="เลือกพนักงาน"
                      :items="employees"
                      class="flex-grow-1"
                    />
                    <VBtn
                      icon
                      variant="text"
                      color="error"
                      size="small"
                      :disabled="form.purchaseCommitteeMembers.length <= 1"
                      @click="removePurchaseCommittee(index)"
                    >
                      <VIcon icon="tabler-trash" size="18" />
                    </VBtn>
                  </div>
                </VCol>

                <VCol cols="12" md="6">
                  <AppTextField
                    v-model="form.purchaseOrderNumber"
                    label="เลขที่คำสั่ง คกก.จัดซื้อ"
                    placeholder="ระบุเลขที่คำสั่ง"
                  />
                </VCol>

                <VCol cols="12" md="6">
                  <AppDateTimePicker
                    v-model="form.purchaseOrderDate"
                    label="วันที่คำสั่ง คกก. จัดซื้อ"
                    placeholder="เลือกวันที่"
                  />
                </VCol>
              </VRow>
            </VCardText>
          </VCard>

          <!-- ═══════════════════════════════════════════════ -->
          <!-- STEP 5: คณะกรรมการตรวจรับ -->
          <!-- ═══════════════════════════════════════════════ -->
          <VCard
            v-show="currentStep === 4"
            class="mb-6"
          >
            <VCardItem>
              <VCardTitle>
                <VIcon icon="tabler-checklist" class="me-2" color="primary" />
                แต่งตั้ง คณะกรรมการตรวจรับ
              </VCardTitle>
              <VCardSubtitle>กำหนดคณะกรรมการตรวจรับ</VCardSubtitle>
            </VCardItem>

            <VDivider />

            <VCardText>
              <VRow>
                <VCol cols="12" md="6">
                  <AppSelect
                    v-model="form.inspectionChairman"
                    label="ประธานคณะกรรมการตรวจรับ"
                    placeholder="เลือกพนักงาน"
                    :items="employees"
                  />
                </VCol>

                <VCol cols="12" md="6">
                  <AppSelect
                    v-model="form.inspectionSecretary"
                    label="กรรมการและเลขาฯ ตรวจรับ"
                    placeholder="เลือกพนักงาน"
                    :items="employees"
                  />
                </VCol>

                <!-- Dynamic: กรรมการตรวจรับ -->
                <VCol cols="12">
                  <div class="d-flex align-center justify-space-between mb-3">
                    <label class="text-body-1 font-weight-medium">กรรมการตรวจรับ</label>
                    <VBtn
                      size="small"
                      variant="tonal"
                      color="primary"
                      prepend-icon="tabler-plus"
                      @click="addInspectionCommittee"
                    >
                      เพิ่มกรรมการ
                    </VBtn>
                  </div>

                  <div
                    v-for="(member, index) in form.inspectionCommitteeMembers"
                    :key="index"
                    class="d-flex align-center gap-3 mb-3"
                  >
                    <span class="text-body-2 text-disabled" style="min-inline-size: 30px;">
                      {{ index + 1 }}.
                    </span>
                    <AppSelect
                      v-model="member.employeeId"
                      placeholder="เลือกพนักงาน"
                      :items="employees"
                      class="flex-grow-1"
                    />
                    <VBtn
                      icon
                      variant="text"
                      color="error"
                      size="small"
                      :disabled="form.inspectionCommitteeMembers.length <= 1"
                      @click="removeInspectionCommittee(index)"
                    >
                      <VIcon icon="tabler-trash" size="18" />
                    </VBtn>
                  </div>
                </VCol>

                <VCol cols="12" md="6">
                  <AppTextField
                    v-model="form.inspectionOrderNumber"
                    label="เลขที่คำสั่ง คกก.ตรวจรับ"
                    placeholder="ระบุเลขที่คำสั่ง"
                  />
                </VCol>

                <VCol cols="12" md="6">
                  <AppDateTimePicker
                    v-model="form.inspectionOrderDate"
                    label="วันที่คำสั่ง คกก. ตรวจรับ"
                    placeholder="เลือกวันที่"
                  />
                </VCol>
              </VRow>
            </VCardText>
          </VCard>

          <!-- ═══════════════════════════════════════════════ -->
          <!-- STEP 6: ผลการ e-bidding -->
          <!-- ═══════════════════════════════════════════════ -->
          <VCard
            v-show="currentStep === 5"
            class="mb-6"
          >
            <VCardItem>
              <VCardTitle>
                <VIcon icon="tabler-trophy" class="me-2" color="primary" />
                ผลการ e-bidding
              </VCardTitle>
              <VCardSubtitle>ข้อมูลผู้ชนะการประกวดราคา</VCardSubtitle>
            </VCardItem>

            <VDivider />

            <VCardText>
              <VRow>
                <VCol cols="12" md="6">
                  <AppSelect
                    v-model="form.bidWinnerId"
                    label="ผู้ชนะการประกวดราคา"
                    placeholder="เลือกบริษัทจากรายการสืบราคา"
                    :items="bidWinnerCompanies"
                  />
                  <div class="text-caption text-disabled mt-1">
                    * เลือกจากบริษัทที่กำหนดในขั้นตอนกำหนดราคากลาง
                  </div>
                </VCol>

                <VCol cols="12" md="6">
                  <AppTextField
                    v-model="form.priceReduction"
                    label="ลดราคาจากราคากลาง"
                    placeholder="0.00"
                    type="number"
                    prefix="฿"
                  />
                </VCol>

                <VCol cols="12" md="6">
                  <AppTextField
                    v-model="form.lowestPrice"
                    label="ราคาที่ต่ำสุด"
                    placeholder="0.00"
                    type="number"
                    prefix="฿"
                  />
                </VCol>

                <VCol cols="12" md="6">
                  <AppTextField
                    v-model="form.biddingRemark"
                    label="หมายเหตุ"
                    placeholder="ระบุหมายเหตุ (ถ้ามี)"
                  />
                </VCol>
              </VRow>
            </VCardText>
          </VCard>

          <!-- ═══════════════════════════════════════════════ -->
          <!-- STEP 7: ข้อมูลสัญญา -->
          <!-- ═══════════════════════════════════════════════ -->
          <VCard
            v-show="currentStep === 6"
            class="mb-6"
          >
            <VCardItem>
              <VCardTitle>
                <VIcon icon="tabler-file-dollar" class="me-2" color="primary" />
                ข้อมูลสัญญา
              </VCardTitle>
              <VCardSubtitle>รายละเอียดข้อมูลสัญญา</VCardSubtitle>
            </VCardItem>

            <VDivider />

            <VCardText>
              <VRow>
                <VCol cols="12" md="6">
                  <AppSelect
                    v-model="form.contractType"
                    label="ประเภทสัญญา"
                    placeholder="เลือกประเภทสัญญา"
                    :items="contractTypeOptions"
                  />
                </VCol>

                <VCol cols="12" md="6">
                  <AppTextField
                    v-model="form.contractNumber"
                    label="เลขที่สัญญา"
                    placeholder="ระบุเลขที่สัญญา"
                  />
                </VCol>

                <VCol cols="12" md="6">
                  <AppDateTimePicker
                    v-model="form.contractSignDate"
                    label="ลงนามสัญญาวันที่"
                    placeholder="เลือกวันที่"
                  />
                </VCol>

                <VCol cols="12" md="3">
                  <AppTextField
                    v-model="form.contractQuantity"
                    label="จำนวน"
                    placeholder="ระบุจำนวน"
                    type="number"
                  />
                </VCol>

                <VCol cols="12" md="3">
                  <AppSelect
                    v-model="form.contractUnit"
                    label="หน่วย"
                    placeholder="เลือกหน่วย"
                    :items="unitOptions"
                  />
                </VCol>

                <VCol cols="12" md="4">
                  <AppTextField
                    v-model="form.contractUnitPrice"
                    label="ราคา/หน่วย"
                    placeholder="0.00"
                    type="number"
                    prefix="฿"
                  />
                </VCol>

                <VCol cols="12" md="4">
                  <AppTextField
                    v-model="form.contractTotalPrice"
                    label="ราคารวม"
                    placeholder="0.00"
                    type="number"
                    prefix="฿"
                  />
                </VCol>

                <VCol cols="12" md="4">
                  <AppTextField
                    v-model="form.contractInstallments"
                    label="จำนวนงวด"
                    placeholder="ระบุจำนวนงวด"
                    type="number"
                  />
                </VCol>

                <VCol cols="12" md="6">
                  <AppDateTimePicker
                    v-model="form.contractStartDate"
                    label="วันที่เริ่มสัญญา"
                    placeholder="เลือกวันที่"
                  />
                </VCol>

                <VCol cols="12" md="6">
                  <AppDateTimePicker
                    v-model="form.contractEndDate"
                    label="วันสิ้นสุดสัญญา"
                    placeholder="เลือกวันที่"
                  />
                </VCol>
              </VRow>
            </VCardText>

            <!-- Sub-section: หลักประกันสัญญา -->
            <VDivider />

            <VCardItem>
              <VCardTitle class="text-body-1">
                <VIcon icon="tabler-shield-check" class="me-2" color="warning" />
                หลักประกันสัญญา
              </VCardTitle>
            </VCardItem>

            <VCardText>
              <VRow>
                <VCol cols="12" md="6">
                  <AppSelect
                    v-model="form.guaranteeType"
                    label="ประเภท"
                    placeholder="เลือกประเภทหลักประกัน"
                    :items="guaranteeTypeOptions"
                  />
                </VCol>

                <VCol cols="12" md="6">
                  <AppTextField
                    v-model="form.guaranteeReference"
                    label="อ้างอิง"
                    placeholder="ระบุเลขที่อ้างอิง"
                  />
                </VCol>

                <VCol cols="12" md="6">
                  <AppTextField
                    v-model="form.guaranteeAmount"
                    label="ยอดเงิน"
                    placeholder="0.00"
                    type="number"
                    prefix="฿"
                  />
                </VCol>

                <VCol cols="12" md="6">
                  <AppTextField
                    v-model="form.guaranteePeriod"
                    label="ระยะประกัน"
                    placeholder="ระบุระยะประกัน"
                  />
                </VCol>
              </VRow>
            </VCardText>
          </VCard>

          <!-- ═══════════════════════════════════════════════ -->
          <!-- STEP 8: ผลการตรวจรับ -->
          <!-- ═══════════════════════════════════════════════ -->
          <VCard
            v-show="currentStep === 7"
            class="mb-6"
          >
            <VCardItem>
              <VCardTitle>
                <VIcon icon="tabler-clipboard-check" class="me-2" color="primary" />
                ผลการตรวจรับ
              </VCardTitle>
              <VCardSubtitle>บันทึกผลการตรวจรับ</VCardSubtitle>
            </VCardItem>

            <VDivider />

            <VCardText>
              <VRow>
                <VCol cols="12" md="6">
                  <AppDateTimePicker
                    v-model="form.inspectionDate"
                    label="วันที่ตรวจรับ"
                    placeholder="เลือกวันที่"
                  />
                </VCol>

                <VCol cols="12" md="6">
                  <AppSelect
                    v-model="form.inspectionResult"
                    label="ผลการตรวจรับ"
                    placeholder="เลือกผลการตรวจรับ"
                    :items="inspectionResultOptions"
                  />
                </VCol>

                <VCol cols="12">
                  <AppTextField
                    v-model="form.inspectionRemark"
                    label="หมายเหตุ"
                    placeholder="ระบุหมายเหตุ (ถ้ามี)"
                  />
                </VCol>

                <VCol cols="12">
                  <AppTextField
                    v-model="form.location"
                    label="Location"
                    placeholder="ระบุสถานที่"
                  />
                </VCol>
              </VRow>
            </VCardText>
          </VCard>

          <!-- 👉 Navigation Buttons -->
          <div class="d-flex justify-space-between align-center">
            <VBtn
              variant="tonal"
              color="secondary"
              :disabled="currentStep === 0"
              prepend-icon="tabler-arrow-left"
              @click="prevStep"
            >
              ย้อนกลับ
            </VBtn>

            <div class="text-body-2 text-disabled">
              ขั้นตอนที่ {{ currentStep + 1 }} จาก {{ steps.length }}
            </div>

            <VBtn
              v-if="currentStep < steps.length - 1"
              color="primary"
              append-icon="tabler-arrow-right"
              @click="nextStep"
            >
              ถัดไป
            </VBtn>

            <VBtn
              v-else
              color="success"
              prepend-icon="tabler-device-floppy"
              :disabled="isProgressDialogVisible"
              @click="create"
            >
              บันทึกข้อมูล
            </VBtn>
          </div>
        </VCol>
      </VRow>
    </VForm>

    <!-- 👉 Progress Dialog -->
    <ProgressDialog
      ref="progressDialogRef"
      v-model:model-value="isProgressDialogVisible"
      progress-message="กำลังประมวลผล, โปรดรอสักครู่..."
      success-title="สำเร็จ!"
      success-message="เพิ่มข้อมูลสัญญาสำเร็จ"
      failure-title="เพิ่มข้อมูลไม่สำเร็จ"
      failure-message="เพิ่มข้อมูลไม่สำเร็จ, โปรดตรวจสอบ!"
      :failure-data="progressDialogFailureDescription"
      @retry="create"
      @cancel="cancelCreate"
      @success="createSuccess"
    />
  </div>
</template>

<style lang="scss" scoped>
.stepper-sidebar {
  position: sticky;
  inset-block-start: 80px;
}

.stepper-item {
  transition: all 0.25s ease;
  border: 1px solid transparent;

  &:hover {
    background-color: rgba(var(--v-theme-on-surface), 0.04);
  }

  &--active {
    background-color: rgba(var(--v-theme-primary), 0.08) !important;
    border-color: rgba(var(--v-theme-primary), 0.3);
  }

  &--completed {
    opacity: 0.85;
  }
}

.bg-light-primary {
  background-color: rgba(var(--v-theme-primary), 0.08);
  border: 1px solid rgba(var(--v-theme-primary), 0.15);
}
</style>
