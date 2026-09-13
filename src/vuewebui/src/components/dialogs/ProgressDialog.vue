<script setup>
const props = defineProps({
  progressMessage: {
    type: String,
    required: false,
  },
  successTitle: {
    type: String,
    required: true,
  },
  successMessage: {
    type: String,
    required: true,
  },
  failureTitle: {
    type: String,
    required: true,
  },
  failureMessage: {
    type: String,
    required: true,
  },
  failureData: {
    type: Object,
    required: false,
  },
  modelValue: {
    type: Boolean,
    required: true,
  },
})

const emit = defineEmits([
  'retry',
  'cancel',
  'success',
  'update:modelValue',
])

const progress = ref(false)
const success = ref(false)
const failure = ref(false)

const startProgress = () => {
  progress.value = true
}

const stopProgress = () => {
  progress.value = false
}

const showSuccess = () => {
  success.value = true
}

const showFailure = () => {
  failure.value = true
}

const closeDialog = () => {
  progress.value = false
  success.value = false
  failure.value = false
  emit('update:modelValue', false)
}

const handleSuccessOk = () => {
  success.value = false
  emit('update:modelValue', false)
  emit('success')
}

const handleRetry = () => {
  failure.value = false
  emit('retry')
}

const handleCancel = () => {
  failure.value = false
  emit('update:modelValue', false)
  emit('cancel')
}

const formettedErrors = computed(() => {
  if (!props.failureData) return []
  return Object.entries(props.failureData).map(([key, messages]) => {
    return { field: key, messages }
  })
})

defineExpose({
  closeDialog,
  startProgress,
  stopProgress,
  showSuccess,
  showFailure,
})
</script>

<template>
  <!-- 👉 Progress Dialog -->
  <VDialog
    v-model="progress"
    width="300"
    persistent
  >
    <VCard
      color="primary"
      width="300"
    >
      <VCardText class="pt-3">
        <VProgressLinear
          indeterminate
          bg-color="rgba(var(--v-theme-surface), 0.1)"
          :height="8"
          class="mb-0 mt-4"
        />
        <p class="mt-4 text-center">
          {{ props.progressMessage }}
        </p>
      </VCardText>
    </VCard>
  </VDialog>

  <!-- 👉 Success Dialog -->
  <VDialog
    v-model="success"
    max-width="500"
    persistent
  >
    <VCard>
      <VCardText class="text-center px-10 py-6">
        <VBtn
          icon
          variant="outlined"
          color="success"
          class="my-4"
          style=" block-size: 88px;inline-size: 88px; pointer-events: none;"
        >
          <VIcon
            icon="tabler-check"
            size="38"
          />
        </VBtn>

        <h6 class="text-lg font-weight-medium mb-4">
          {{ props.successTitle }}
        </h6>

        <p>{{ props.successMessage }}</p>

        <VBtn
          color="success"
          @click="handleSuccessOk"
        >
          โอเค
        </VBtn>
      </VCardText>
    </VCard>
  </VDialog>

  <!-- 👉 Failure Dialog -->
  <VDialog
    v-model="failure"
    max-width="500"
    persistent
  >
    <VCard>
      <VCardText class="text-center px-10 py-6">
        <VBtn
          icon
          variant="outlined"
          color="error"
          class="my-4"
          style=" block-size: 88px;inline-size: 88px; pointer-events: none;"
        >
          <span class="text-5xl font-weight-light">X</span>
        </VBtn>

        <h6 class="text-lg font-weight-medium mb-4">
          {{ props.failureTitle }}
        </h6>

        <p>{{ props.failureMessage }}</p>
        <div
          v-if="formettedErrors.length"
          class="text-left"
        >
          <div
            v-for="error in formettedErrors"
            :key="error.field"
          >
            <ul class="list-style-none">
              <li
                v-for="message in error.messages"
                :key="message"
              >
                {{ message }}
              </li>
            </ul>
          </div>
        </div>
      </VCardText>
      <VCardText class="d-flex align-center justify-center gap-2">
        <VBtn
          color="primary"
          variant="elevated"
          @click="handleRetry"
        >
          ลองใหม่
        </VBtn>
        <VBtn
          color="secondary"
          variant="tonal"
          @click="handleCancel"
        >
          ยกเลิก
        </VBtn>
      </VCardText>
    </VCard>
  </VDialog>
</template>
