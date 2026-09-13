<script setup>
import { requiredValidator } from '@/@core/utils/validators'
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { VForm } from 'vuetify/components/VForm'

// eslint-disable-next-line import/extensions
import moment from 'moment/min/moment-with-locales.js'

moment.locale('th')

const router = useRouter()
const route = useRoute('sectors-edit-id')

const isProgressDialogVisible = ref(false)
const progressDialogRef = ref(null)
const progressDialogFailureDescription = ref(null)

const refForm = ref()
const id = ref(null)
const name = ref(null)
const code = ref(null)

const { data: sectorData } = await useApi(`sectors/${ route.params.id }`)
if(sectorData.value) {
  id.value = sectorData.value.id
  code.value = sectorData.value.code
  name.value = sectorData.value.name
}
//alert('${ route.params.id }');
const update = async () => {

  if (!refForm.value) return
  const isValid = await refForm.value.validate()
  if (!isValid.valid) {
    return
  }

  isProgressDialogVisible.value = true

  var sectorData = {
    id: id.value,
    code: code.value,  
    name: name.value,
  }

  try {
    var response = await $api(`/sectors/${ route.params.id }`, { method: "PUT", body: sectorData })
    progressDialogRef.value.stopProgress()
    progressDialogRef.value.showSuccess()
    isProgressDialogVisible.value = false
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

const cancelUpdateSector = () => {
  progressDialogRef.value.closeDialog()
}

const updateSectorSuccess = () => {
  router.push(`/sectors/view/${id.value}`)
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
      <span class="text-medium-emphasis">แก้ไขข้อมูลกลุ่มงาน</span>
    </div>
  <!-- </VCard> -->
  </div>
  <div v-if="sectorData">
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
                    <VCardTitle>แก้ไขข้อมูลกลุ่มงาน</VCardTitle>
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
        @retry="update"
        @cancel="cancelUpdateSector"
        @success="updateSectorSuccess"
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
