<script setup>
import { requiredValidator } from '@/@core/utils/validators'
import { ref } from 'vue'
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
const createdSectorId = ref(null)
const name = ref(null)
const code = ref(null)

const create = async () => {

  if (!refForm.value) return
  const isValid = await refForm.value.validate()
  if (!isValid.valid) {
    return
  }

  isProgressDialogVisible.value = true

  var doctorData = {
    code: code.value,
    name: name.value,
  }

  try {
    var response = await $api(`/sectors`, { method: "POST", body: sectorData })
    if(response) {
      progressDialogRef.value.stopProgress()
      progressDialogRef.value.showSuccess()
      isProgressDialogVisible.value = false
      createdSectorId.value = response
    }
  } catch (error) {
    setTimeout(() => {
      if (error.data) {
        progressDialogFailureDescription.value = error.data?.errors || [{ "error": "ขออภัย, ระบบเกิดข้อผิดพลาด." }]
      } else if (error.request) {
        progressDialogFailureDescription.value = "ขออภัย, ไม่สามารถให้บริการได้ในขณะนี้, โปรดลองใหม่อีกครั้ง"
      } else {
        progressDialogFailureDescription.value = error.message
      }

      progressDialogRef.value.stopProgress()
      progressDialogRef.value.showFailure()
      isProgressDialogVisible.value = false
    }, 1000)
  }
}

const cancelCreateSector = () => {
  progressDialogRef.value.closeDialog()
}

const createSectorSuccess = () => {
  router.push(`/sectors/view/${createdSectorId.value}`)
}
</script>

<template>
  <div class="d-flex flex-column mb-6">
  <!-- <VCard class="mb6"> -->
  <div class="text-body-2 d-flex align-center gap-1 mt-1">
      <IconBtn><VIcon icon="tabler-home" /></IconBtn>
      <RouterLink to="/" class="text-primary">หน้าหลัก</RouterLink>
      <span>>></span>
      <RouterLink to="/sectors/list" class="text-primary">รายการข้อมูลกลุ่มงาน</RouterLink>
      <span>>></span>
      <span class="text-medium-emphasis">เพิ่มข้อมูลกลุ่มงาน</span>
    </div>
  <!-- </VCard> -->
  </div>
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
                  <VCardTitle>เพิ่มกลุ่มงาน</VCardTitle>
                </div>

                <div class="d-flex gap-4 align-center flex-wrap">
                  <VBtn
                    type="submit"
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
        <VCol md="6">
          <VCard
            class="mb-6"
            title="ข้อมูลกลุ่มงาน"
          >
            <VCardText>
              <VRow>
                <VCol cols="12">
                  <AppTextField
                    v-model="code"
                    label="รหัสกลุ่มงาน"
                    placeholder="รหัสกลุ่มงาน"
                    :rules="[requiredValidator]"
                  />
                </VCol>
                <VCol cols="12">
                  <AppTextField
                    v-model="name"
                    label="ชื่อกลุ่มงาน"
                    placeholder="ชื่อกลุ่มงาน"
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
      success-message="เพิ่มข้อมูลสำเร็จ"
      failure-title="เพิ่มข้อมูลไม่สำเร็จ"
      failure-message="เพิ่มข้อมูลไม่สำเร็จ, โปรดตรวจสอบ!"
      :failure-data="progressDialogFailureDescription"
      @retry="create"
      @cancel="cancelCreateSector"
      @success="createSectorSuccess"
    />
  </div>
</template>
