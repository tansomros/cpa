<script setup>
import { $api } from '@/utils/api'
import { ref, watch } from 'vue'

const router = useRouter()

const searchQuery = ref('')
const statusFilter = ref(true)
const itemsPerPage = ref(10)
const page = ref(1)
const items = ref([])
const totalItems = ref(0)
const isLoading = ref(false)
const errorMessage = ref(null)

const headers = [
  { title: 'รหัสร้านยา', key: 'code', sortable: false },
  { title: 'ชื่อร้านยา', key: 'name', sortable: false },
  { title: 'เลขที่ใบอนุญาต', key: 'licenseNo', sortable: false },
  { title: 'กลุ่มร้านยา', key: 'pharmacyGroupName', sortable: false },
  { title: 'จังหวัด', key: 'provinceName', sortable: false },
  { title: 'เบอร์โทรสำนักงาน', key: 'office_Tel', sortable: false },
  { title: 'สถานะ', key: 'isActive', sortable: false },
  { title: 'จัดการ', key: 'actions', sortable: false },
]

const statusOptions = [
  { value: true, title: 'ใช้งาน' },
  { value: false, title: 'ไม่ใช้งาน' },
  { value: null, title: 'ทั้งหมด' },
]

const fetchItems = async () => {
  isLoading.value = true
  errorMessage.value = null

  const query = { page: page.value, limit: itemsPerPage.value }
  if (searchQuery.value.trim()) query.search = searchQuery.value.trim()
  if (statusFilter.value !== null) query.isActive = statusFilter.value

  try {
    const result = await $api('/pharmacies', { method: 'GET', query })

    items.value = result.items ?? []
    totalItems.value = result.totalCount ?? 0
  } catch {
    items.value = []
    totalItems.value = 0
    errorMessage.value = 'โหลดรายการร้านขายยาไม่สำเร็จ'
  } finally {
    isLoading.value = false
  }
}

let searchTimer

watch(searchQuery, () => {
  clearTimeout(searchTimer)
  searchTimer = setTimeout(() => {
    page.value = 1
    fetchItems()
  }, 300)
})

watch([statusFilter, itemsPerPage], () => {
  page.value = 1
  fetchItems()
})

watch(page, fetchItems)

fetchItems()
</script>

<template>
  <VCard>
    <VCardItem class="pb-4">
      <VCardTitle>รายการร้านขายยา</VCardTitle>
    </VCardItem>

    <VCardText class="d-flex flex-wrap gap-4">
      <AppSelect
        v-model="itemsPerPage"
        :items="[10, 25, 50, 100]"
        style="inline-size: 6.25rem;"
      />
      <AppSelect
        v-model="statusFilter"
        :items="statusOptions"
        style="inline-size: 10rem;"
        data-testid="pharmacies-status-filter"
      />
      <VSpacer />
      <div style="inline-size: 15.625rem;">
        <AppTextField
          v-model="searchQuery"
          placeholder="ค้นหารหัส ชื่อ ใบอนุญาต"
          data-testid="pharmacies-search-input"
        />
      </div>
      <VBtn
        prepend-icon="tabler-plus"
        data-testid="pharmacies-create-button"
        @click="router.push({ name: 'pharmacies-create' })"
      >
        เพิ่มร้านขายยา
      </VBtn>
    </VCardText>

    <VAlert
      v-if="errorMessage"
      type="error"
      variant="tonal"
      class="mx-6 mb-4"
    >
      {{ errorMessage }}
    </VAlert>

    <VDivider />

    <VDataTableServer
      v-model:items-per-page="itemsPerPage"
      v-model:page="page"
      :items="items"
      :items-length="totalItems"
      :headers="headers"
      :loading="isLoading"
      item-value="id"
      class="text-no-wrap"
      data-testid="pharmacies-table"
    >
      <template #item.licenseNo="{ item }">
        {{ item.licenseNo || '-' }}
      </template>
      <template #item.pharmacyGroupName="{ item }">
        {{ item.pharmacyGroupName || '-' }}
      </template>
      <template #item.provinceName="{ item }">
        {{ item.provinceName || '-' }}
      </template>
      <template #item.office_Tel="{ item }">
        {{ item.office_Tel || '-' }}
      </template>
      <template #item.isActive="{ item }">
        <VChip
          :color="item.isActive ? 'success' : 'error'"
          size="small"
          label
        >
          {{ item.isActive ? 'ใช้งาน' : 'ไม่ใช้งาน' }}
        </VChip>
      </template>
      <template #item.actions="{ item }">
        <IconBtn @click="router.push({ name: 'pharmacies-view-id', params: { id: item.id } })">
          <VIcon icon="tabler-eye" />
        </IconBtn>
        <IconBtn @click="router.push({ name: 'pharmacies-edit-id', params: { id: item.id } })">
          <VIcon icon="tabler-pencil" />
        </IconBtn>
      </template>

      <template #bottom>
        <TablePagination
          v-model:page="page"
          :items-per-page="itemsPerPage"
          :total-items="totalItems"
        />
      </template>
    </VDataTableServer>
  </VCard>
</template>
