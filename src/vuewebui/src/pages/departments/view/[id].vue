<script setup>
import { useApi } from '@/composables/useApi'

// eslint-disable-next-line import/extensions
import moment from 'moment/min/moment-with-locales.js'

moment.locale('th')

const route = useRoute('departments-view-id')
const { data: departmentData } = await useApi(`departments/${ route.params.id }`)
</script>

<template>
    <div class="d-flex flex-column mb-6">
    <div class="text-body-2 d-flex align-center gap-1 mt-1">
      <IconBtn><VIcon icon="tabler-home" /></IconBtn>
      <RouterLink to="/" class="text-primary">หน้าหลัก</RouterLink>
      <span>>></span>
      <RouterLink to="/departments/list" class="text-primary">รายการข้อมูลแผนก</RouterLink>
      <span>>></span>
      <span class="text-medium-emphasis">ข้อมูลแผนก</span>
    </div>
  </div>
  <div v-if="departmentData">
    <VRow>
      <VCol cols="12">
        <VCard>
          <VCardText class="text-center pt-12">
            <!-- 👉 Department name -->
            <h5 class="text-h5 mt-4">
              {{ departmentData.name }}
            </h5>

            <!-- 👉 Code chip -->
            <VChip
              label
              color="primary"
              size="small"
              class="text-capitalize mt-4"
            >
              {{ departmentData.code }}
            </VChip>
          </VCardText>

          <VCardText>
            <VList class="card-list">
              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  รหัสแผนก
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ departmentData.code }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  ชื่อแผนก
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ departmentData.name }}</span>
                </template>
              </VListItem>

              <VListItem>
                <VListItemTitle class="text-sm font-weight-semibold">
                  รหัสฝ่าย (DivisionId)
                </VListItemTitle>
                <template #append>
                  <span class="text-body-1">{{ departmentData.divisionId }}</span>
                </template>
              </VListItem>
            </VList>
          </VCardText>

          <!-- 👉 Edit button -->
          <VCardText class="d-flex justify-center gap-x-4">
            <VBtn
              variant="elevated"
              :to="{ name: 'departments-edit-id', params: { id: departmentData.id } }"
            >
              แก้ไข
            </VBtn>
          </VCardText>
        </VCard>
      </VCol>
    </VRow>
  </div>
  <div v-else>
    <VAlert
      type="error"
      variant="tonal"
    >
      ไม่พบข้อมูล, โปรดเลือกข้อมูลที่ต้องการแสดงในหน้า "รายการ"
    </VAlert>
  </div>
</template>
