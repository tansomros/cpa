<script setup>
import { $api } from '@/utils/api'
import { emptyPharmacyForm, extractValidationErrors, formToPayload } from '@/utils/pharmacyForm'
import { ref } from 'vue'

const router = useRouter()

const form = ref(emptyPharmacyForm())
const isSaving = ref(false)
const serverErrors = ref({})
const errorMessage = ref(null)

const save = async () => {
  isSaving.value = true
  serverErrors.value = {}
  errorMessage.value = null

  try {
    const id = await $api('/pharmacies', {
      method: 'POST',
      body: formToPayload(form.value),
    })

    router.push({ name: 'pharmacies-view-id', params: { id } })
  } catch (error) {
    serverErrors.value = extractValidationErrors(error)
    errorMessage.value = error?.data?.detail ?? 'บันทึกข้อมูลร้านขายยาไม่สำเร็จ'
  } finally {
    isSaving.value = false
  }
}
</script>

<template>
  <VCard>
    <VCardItem>
      <VCardTitle>เพิ่มร้านขายยา</VCardTitle>
    </VCardItem>
    <VCardText>
      <VAlert
        v-if="errorMessage"
        type="error"
        variant="tonal"
        class="mb-4"
      >
        {{ errorMessage }}
      </VAlert>
      <PharmacyForm
        v-model="form"
        :saving="isSaving"
        :server-errors="serverErrors"
        @submit="save"
        @cancel="router.push({ name: 'pharmacies-list' })"
      />
    </VCardText>
  </VCard>
</template>
