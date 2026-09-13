<script setup>
import { requiredValidator } from '@/@core/utils/validators'
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { VForm } from 'vuetify/components/VForm'

// eslint-disable-next-line import/extensions
import moment from 'moment/min/moment-with-locales.js'

moment.locale('th')

const router = useRouter()
const route = useRoute('departments-edit-id')

const isProgressDialogVisible = ref(false)
const progressDialogRef = ref(null)
const progressDialogFailureDescription = ref(null)

const refForm = ref()
const id = ref(null)
const name = ref(null)
const code = ref(null)
const divisionId = ref(null)

// โหลดรายการฝ่ายสำหรับ dropdown
const { data: divisionData } = await useApi(`/divisions/GetAllDivisionList`, { method: 'GET' })
const divisions = computed(() => {
  if (!divisionData.value?.divisions) return []
  return divisionData.value.divisions.map(d => ({
    title: d.name,
    value: d.id,
  }))
})

// โหลดข้อมูลแผนกที่ต้องการแก้ไข
const { data: departmentData } = await useApi(`departments/${ route.params.id }`)
if(departmentData.value) {
  id.value = departmentData.value.id
  code.value = departmentData.value.code
  name.value = departmentData.value.name
  divisionId.value = departmentData.value.divisionId
}

const update = async () => {

  if (!refForm.value) return
  const isValid = await refForm.value.validate()
  if (!isValid.valid) {
    return
  }

  isProgressDialogVisible.value = true
  progressDialogRef.value.startProgress()

  var departmentPayload = {
    id: id.value,
    code: code.value,
    name: name.value,
    divisionId: divisionId.value,
  }

  try {
    var response = await $api(`/departments/${ route.params.id }`, { method: "PUT", body: departmentPayload })
    progressDialogRef.value.stopProgress()
    progressDialogRef.value.showSuccess()
  } catch (error) {
    if (error.data) {
      progressDialogFailureDescription.value = error.data?.errors || [{ "error": "ขออภัย, ระบบเกิดข้อผิดพลาด." }]
    } else if (error.request) {
      progressDialogFailureDescription.value = "ขออภัย, ไม่สามารถให้บริการได้ในขณะนี้, โปรดลองใหม่อีกครั้ง"
    } else {
      progressDialogFailureDescription.value = error.message
    }

    progressDialogRef.value.stopProgress()
    progressDialogRef.value.showFailure()
  }
}

const cancelUpdateDepartment = () => {
  progressDialogRef.value.closeDialog()
  isProgressDialogVisible.value = false
}

const updateDepartmentSuccess = () => {
  isProgressDialogVisible.value = false
  if (id.value) {
    try {
      const existing = JSON.parse(sessionStorage.getItem('departments_highlights') || '[]')
      const map = new Map(existing)
      map.set(id.value, { timestamp: Date.now(), type: 'edit' })
      sessionStorage.setItem('departments_highlights', JSON.stringify(Array.from(map.entries())))
    } catch (e) {
      console.error(e)
    }
  }
  router.push('/departments/list')
}
</script>

<template>
  <div class="d-flex flex-column mb-6">
    <div class="text-body-2 d-flex align-center gap-1 mt-1">
      <IconBtn><VIcon icon="tabler-home" /></IconBtn>
      <RouterLink to="/" class="text-primary">หน้าหลัก</RouterLink>
      <span>>></span>
      <RouterLink to="/departments/list" class="text-primary">รายการข้อมูลแผนก</RouterLink>
      <span>>></span>
      <span class="text-medium-emphasis">แก้ไขข้อมูลแผนก</span>
    </div>
  </div>
  <div v-if="departmentData">
    <div>
      <VForm
        ref="refForm"
        @submit.prevent
      >
        <VRow>
          <VCol md="12">
            <VCard class="mb6">
              <VCardItem>
                <div class="d-flex flex-wrap justify-start justify-sm-space-between gap-y-4 gap-x-6">
                  <div class="d-flex flex-column justify-center">
                    <VCardTitle>แก้ไขข้อมูลแผนก</VCardTitle>
                  </div>

                  <div class="d-flex gap-4 align-center flex-wrap">
                    <VBtn
                      type="submit"
                      :disabled="isProgressDialogVisible"
                      @click="update"
                    >
                      บันทึกการเปลี่ยนแปลง
                    </VBtn>
                  </div>
                </div>
              </VCardItem>
            </VCard>
          </VCol>
        </VRow>
    
        <VRow>
          <VCol md="6">
            <VCard
              class="mb-6"
            >
              <VCardText>
                <VRow>
                  <VCol cols="12">
                    <AppTextField
                      v-model="code"
                      label="รหัสแผนก"
                      placeholder="รหัสแผนก"
                      :rules="[requiredValidator]"
                    />
                  </VCol>
                  <VCol cols="12">
                    <AppTextField
                      v-model="name"
                      label="ชื่อแผนก"
                      placeholder="ชื่อแผนก"
                      :rules="[requiredValidator]"
                    />
                  </VCol>
                  <VCol cols="12">
                    <AppSelect
                      v-model="divisionId"
                      label="ฝ่าย"
                      placeholder="เลือกฝ่าย"
                      :items="divisions"
                      :rules="[requiredValidator]"
                    />
                  </VCol>
                </VRow>
              </VCardText>
            </VCard>
          </VCol>
        </VRow>
      </VForm>
      <ProgressDialog
        ref="progressDialogRef"
        v-model:model-value="isProgressDialogVisible"
        progress-message="กำลังประมวลผล, โปรดรอสักครู่..."
        success-title="สำเร็จ!"
        success-message="แก้ไขข้อมูลสำเร็จ"
        failure-title="แก้ไขข้อมูลไม่สำเร็จ"
        failure-message="แก้ไขข้อมูลไม่สำเร็จ, โปรดตรวจสอบ!"
        :failure-data="progressDialogFailureDescription"
        @retry="update"
        @cancel="cancelUpdateDepartment"
        @success="updateDepartmentSuccess"
      />
    </div>
  </div>
  <div v-else>
    <VAlert
      type="error"
      variant="tonal"
    >
      ไม่พบข้อมูล, โปรดเลือกข้อมูลที่ต้องการแก้ไขในหน้า "รายการ"
    </VAlert>
  </div>
</template>
