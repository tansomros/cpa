<script setup>
//import { toBuddhistYear } from '@/utils/dateUtils'

// eslint-disable-next-line import/extensions
//import moment from 'moment/min/moment-with-locales.js'
//import { ref } from 'vue'

//moment.locale('th')

const searchQuery = ref('')
const itemsPerPage = ref(10)
const page = ref(1)
const sortBy = ref()
const orderBy = ref()
const selectedRows = ref([])
const selectedDepartments = ref()
const isConfirmProgressDialogVisible = ref(false)
const progressDialogRef = ref(null)
const progressDialogFailureDescription = ref(null)


const headers = [
  {
    title: "รหัส",
    key: "code",
  },
  {
    title: "ชื่อ",
    key: "name",
  },
  {
    title: "สถานะ",
    key: "isActive",

  },
  {
    title: "จัดการ",
    key: "actions",
    sortable: false,
  },
]

const { data: sectorsData } = await useApi(`/sectors/getsectorlist`, { method: 'GET' })

const sectors = computed(() => sectorsData.value.sectors)

</script>

<template>
  <section>
    <VCard class="mb-6">
      <VCardItem class="pb-4">
        <VCardTitle>รายการข้อมูลฝ่าย</VCardTitle>
      </VCardItem>

      <VCardText class="d-flex flex-wrap gap-4">
        <div class="me-3 d-flex gap-3">
          <AppSelect :model-value="itemsPerPage" :items="[
            { value: 10, title: '10' },
            { value: 25, title: '25' },
            { value: 50, title: '50' },
            { value: 100, title: '100' },
          ]" style="inline-size: 6.25rem;" @update:model-value="itemsPerPage = parseInt($event, 10)" />
        </div>
        <VSpacer />

        <div class="app-user-search-filter d-flex align-center flex-wrap gap-4">
          <!-- 👉 Search  -->
          <div style="inline-size: 15.625rem;">
            <AppTextField v-model="searchQuery" placeholder="ค้นหา" />
          </div>

          <!-- 👉 Add user button -->
          <!--<RouterLink :to="{ name: 'departments-create' }">
            <VBtn prepend-icon="tabler-plus">
              เพิ่มแผนก
            </VBtn>
          </RouterLink>-->
        </div>
      </VCardText>

      <VDivider />

      <!-- SECTION datatable -->
      <VDataTableServer v-model:items-per-page="itemsPerPage" v-model:model-value="selectedRows" v-model:page="page"
        :items="sectors" item-value="id" :items-length="totalSectors" :headers="headers" class="text-no-wrap"
        show-select @update:options="updateOptions">
        <!-- Sectors -->
        <template #item.sectors="{ item }">
          <div class="d-flex align-center gap-x-4">
            <div class="d-flex flex-column">
              <h6 class="text-base">{{ item.name }}
              </h6>
            </div>
          </div>
        </template>

        <template #item.code="{ item }">
          <div v-if="item.code">
            {{ item.code }}
          </div>
        </template>

        <!-- Actions -->
        <template #item.actions="{ item }">
          <IconBtn>
            <VIcon icon="tabler-eye" />
          </IconBtn>

          <RouterLink>
            <IconBtn>
              <VIcon icon="tabler-pencil" />
            </IconBtn>
          </RouterLink>

          <VBtn icon variant="text" color="medium-emphasis">
            <VIcon icon="tabler-dots-vertical" />
            <VMenu activator="parent">
              <VList>
                <VListItem>
                  <template #prepend>
                    <VIcon icon="tabler-eye" />
                  </template>

                  <VListItemTitle>แสดง</VListItemTitle>
                </VListItem>

                <VListItem>
                  <template #prepend>
                    <VIcon icon="tabler-pencil" />
                  </template>
                  <VListItemTitle>แก้ไข</VListItemTitle>
                </VListItem>
              </VList>
            </VMenu>
          </VBtn>
        </template>

        <!-- pagination -->
        <template #bottom>
          <TablePagination v-model:page="page" :items-per-page="itemsPerPage" :total-items="totalDoctors" />
        </template>
      </VDataTableServer>

      <ConfirmProgressDialog ref="progressDialogRef" v-model:model-value="isConfirmProgressDialogVisible"
        confirm-title="โปรดยืนยันการลบข้อมูล" confirm-message="ข้อมูลที่เกี่ยวข้องจะถูกลบทั้งหมด"
        progress-message="กำลังประมวลผล, โปรดรอสักครู่..." success-title="ลบข้อมูลสำเร็จ!"
        success-message="ลบข้อมูลสำเร็จ" failure-title="ลบข้อมูลไม่สำเร็จ"
        failure-message="ลบข้อมูลไม่สำเร็จ, โปรดตรวจสอบ!" :failure-data="progressDialogFailureDescription"
        @confirm="deleteDoctor(selectedDoctor.id)" @retry="deleteDoctor(selectedDoctor.id)"
        @cancel="cancelDeleteDoctor" />
    </VCard>
  </section>
</template>
