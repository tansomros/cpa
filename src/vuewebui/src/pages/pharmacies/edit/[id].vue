<script setup>
import { $api } from '@/utils/api'
import { extractValidationErrors, formToPayload, pharmacyToForm } from '@/utils/pharmacyForm'
import { onMounted, ref } from 'vue'

const route = useRoute()
const router = useRouter()

const id = Number(route.params.id)
const form = ref(null)
const rowVersion = ref(null)
const isSaving = ref(false)
const serverErrors = ref({})
const errorMessage = ref(null)
const isConflict = ref(false)

const load = async () => {
  const pharmacy = await $api(`/pharmacies/${id}`, { method: 'GET' })

  form.value = pharmacyToForm(pharmacy)
  rowVersion.value = pharmacy.rowVersion
  isConflict.value = false
  errorMessage.value = null
}

onMounted(async () => {
  try {
    await load()
  } catch {
    errorMessage.value = 'ไม่พบข้อมูลร้านขายยา'
  }
})

const save = async () => {
  isSaving.value = true
  serverErrors.value = {}
  errorMessage.value = null

  try {
    await $api(`/pharmacies/${id}`, {
      method: 'PUT',
      body: { ...formToPayload(form.value), id, rowVersion: rowVersion.value },
    })

    router.push({ name: 'pharmacies-view-id', params: { id } })
  } catch (error) {
    isConflict.value = error?.status === 409
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
      <VCardTitle>แก้ไขร้านขายยา</VCardTitle>
    </VCardItem>
    <VCardText>
      <VAlert
        v-if="errorMessage"
        type="error"
        variant="tonal"
        class="mb-4"
      >
        {{ errorMessage }}
        <template
          v-if="isConflict"
          #append
        >
          <VBtn
            size="small"
            variant="text"
            @click="load"
          >
            โหลดข้อมูลล่าสุด
          </VBtn>
        </template>
      </VAlert>
      <PharmacyForm
        v-if="form"
        :key="rowVersion"
        v-model="form"
        show-status
        :saving="isSaving"
        :server-errors="serverErrors"
        @submit="save"
        @cancel="router.push({ name: 'pharmacies-view-id', params: { id } })"
      />
    </VCardText>
  </VCard>
</template>
