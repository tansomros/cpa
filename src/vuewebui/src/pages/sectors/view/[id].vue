<script setup>
import { useApi } from '@/composables/useApi'

// eslint-disable-next-line import/extensions
import moment from 'moment/min/moment-with-locales.js'

moment.locale('th')

const route = useRoute('sectors-view-id')
const { data: sectorsData } = await useApi(`sectors/${ route.params.id }`)

//alert(sectorsData.value.name);
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
      <span class="text-medium-emphasis">ข้อมูลกลุ่มงาน</span>
    </div>
    <!-- </VCard> -->
  </div>
  <div v-if="sectorsData">
    <VRow>
      <VCol cols="12">
        <VCard>
          <VCardText class="text-center pt-12">
            <!-- 👉 User fullName -->
            <h5 class="text-h5 mt-4">
              {{ sectorsData.name }}
            </h5>

            <!-- 👉 Role chip -->
            <VChip
              label
              color="primary"
              size="small"
              class="text-capitalize mt-4"
            >
             .........
            </VChip>
          </VCardText>


          <!-- 👉 Edit and Suspend button -->
          <VCardText class="d-flex justify-center gap-x-4">
            <!-- <VBtn
              variant="elevated"
                  
              :to="{name: 'sectors-edit-id', params: { id:sectorsData.id }}"
            >
              แก้ไข
            </VBtn> -->
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
