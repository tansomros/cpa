<script setup>
import { toBuddhistYear } from '@/utils/dateUtils'
import { formatCurrency } from '@/utils/numberUtils'

// eslint-disable-next-line import/extensions
import moment from 'moment/min/moment-with-locales.js'

moment.locale('th')

const searchQuery = ref('')
const itemsPerPage = ref(10)
const page = ref(1)
const sortBy = ref()
const orderBy = ref()
const selectedRows = ref([])
const selectedStatus = ref()
const selectedContract = ref()
const isConfirmProgressDialogVisible = ref(false)
const progressDialogRef = ref(null)
const progressDialogFailureDescription = ref(null)

// 👉 Contract status options
const contractStatusOptions = [
  { title: 'ทั้งหมด', value: '' },
  { title: 'มีผลบังคับ', value: 'active' },
  { title: 'ใกล้หมดอายุ', value: 'expiring' },
  { title: 'หมดอายุ', value: 'expired' },
  { title: 'ร่าง', value: 'draft' },
  { title: 'ยกเลิก', value: 'cancelled' },
]

// 👉 Status color/icon mapping
const resolveContractStatusVariant = status => {
  const statusMap = {
    active: { variant: 'success', icon: 'tabler-circle-check' },
    expiring: { variant: 'warning', icon: 'tabler-clock' },
    expired: { variant: 'error', icon: 'tabler-alert-circle' },
    draft: { variant: 'secondary', icon: 'tabler-file' },
    cancelled: { variant: 'info', icon: 'tabler-ban' },
  }

  return statusMap[status] || { variant: 'secondary', icon: 'tabler-file' }
}

const headers = [
  {
    title: 'สถานะ',
    key: 'status',
    width: 70,
    sortable: false,
  },
  {
    title: 'ชื่อสัญญา/รายการ',
    key: 'details',
    minWidth: 180,
  },
  {
    title: 'บริษัท/ห้าง/ร้าน',
    key: 'bidWinnerId',
    minWidth: 120,
  },
  {
    title: 'ประเภท',
    key: 'contractTypeId',
    width: 100,
  },
  {
    title: 'เริ่มสัญญา',
    key: 'startDate',
    width: 100,
  },
  {
    title: 'สิ้นสุด',
    key: 'endDate',
    width: 100,
  },
  {
    title: 'วงเงิน',
    key: 'budget',
    width: 110,
  },
  {
    title: 'คงเหลือ',
    key: 'budgetDiff',
    width: 110,
  },
  {
    title: 'แก้ไขล่าสุด',
    key: 'lastModified',
    width: 100,
  },
  {
    title: 'จัดการ',
    key: 'actions',
    width: 80,
    sortable: false,
  },
]

// 👉 Fetch contracts
const {
  data: ContractsData,
  execute: fetchContracts,
} = await useApi(createUrl('/contracts/paginated', {
  query: {
    searchTerm: searchQuery,
    length: itemsPerPage,
    page,
    sortBy,
    orderBy,
    status: selectedStatus,
  },
}))

const contracts = computed(() => ContractsData.value?.items ?? [])
const totalContracts = computed(() => ContractsData.value?.totalCount ?? 0)

// 👉 Summary stats (computed from response or mocked)
const summaryStats = computed(() => {
  const data = ContractsData.value
  return [
    {
      title: 'สัญญาทั้งหมด',
      value: data?.totalCount ?? 0,
      icon: 'tabler-file-dollar',
      color: 'primary',
    },
    {
      title: 'มีผลบังคับ',
      value: data?.activeCount ?? 0,
      icon: 'tabler-circle-check',
      color: 'success',
    },
    {
      title: 'ใกล้หมดอายุ',
      value: data?.expiringCount ?? 0,
      icon: 'tabler-clock',
      color: 'warning',
    },
    {
      title: 'หมดอายุ',
      value: data?.expiredCount ?? 0,
      icon: 'tabler-alert-circle',
      color: 'error',
    },
  ]
})

// 👉 Format date to Buddhist era
const formatDate = dateStr => {
  if (!dateStr) return '-'
  const m = moment(dateStr)
  return m.isValid() ? toBuddhistYear(m, 'DD MMM YYYY') : '-'
}

// 👉 Update table options
const updateOptions = options => {
  sortBy.value = options.sortBy?.[0]?.key
  orderBy.value = options.sortBy?.[0]?.order
}

// 👉 Delete contract
const deleteContract = async id => {
  try {
    progressDialogRef.value?.setProgress()
    await useApi(`/contracts/${id}`, { method: 'DELETE' })
    progressDialogRef.value?.setSuccess()
    fetchContracts()
  }
  catch {
    progressDialogFailureDescription.value = 'ไม่สามารถลบข้อมูลสัญญาได้'
    progressDialogRef.value?.setFailure()
  }
}

const cancelDeleteContract = () => {
  selectedContract.value = null
}

const confirmDeleteContract = contract => {
  selectedContract.value = contract
  isConfirmProgressDialogVisible.value = true
}
</script>

<template>
  <section>
    <!-- 👉 Summary Cards -->
    <VCard class="mb-6">
      <VCardText>
        <VRow>
          <VCol
            v-for="stat in summaryStats"
            :key="stat.title"
            cols="12"
            sm="6"
            md="3"
          >
            <div class="d-flex align-center gap-x-4">
              <VAvatar
                :color="stat.color"
                variant="tonal"
                rounded
                size="42"
              >
                <VIcon
                  :icon="stat.icon"
                  size="24"
                />
              </VAvatar>
              <div>
                <span class="text-body-1 text-high-emphasis font-weight-medium">{{ stat.value }}</span>
                <div class="text-body-2">
                  {{ stat.title }}
                </div>
              </div>
            </div>
          </VCol>
        </VRow>
      </VCardText>
    </VCard>

    <!-- 👉 Contracts Table -->
    <VCard>
      <VCardItem class="pb-4">
        <VCardTitle>รายการข้อมูลสัญญา</VCardTitle>
      </VCardItem>

      <VDivider />

      <!-- 👉 Table Toolbar -->
      <VCardText class="d-flex flex-wrap gap-4">
        <div class="me-3 d-flex gap-3">
          <AppSelect
            :model-value="itemsPerPage"
            :items="[
              { value: 10, title: '10' },
              { value: 25, title: '25' },
              { value: 50, title: '50' },
              { value: 100, title: '100' },
            ]"
            style="inline-size: 6.25rem;"
            @update:model-value="itemsPerPage = parseInt($event, 10)"
          />
        </div>

        <VSpacer />

        <div class="app-user-search-filter d-flex align-center flex-wrap gap-4">
          <!-- 👉 Status Filter -->
          <div style="inline-size: 10rem;">
            <AppSelect
              v-model="selectedStatus"
              placeholder="สถานะ"
              :items="contractStatusOptions"
              clearable
              clear-icon="tabler-x"
            />
          </div>

          <!-- 👉 Search -->
          <div style="inline-size: 15.625rem;">
            <AppTextField
              v-model="searchQuery"
              placeholder="ค้นหา"
            />
          </div>

          <!-- 👉 Add Contract Button -->
          <RouterLink :to="{ name: 'contracts-create' }">
            <VBtn prepend-icon="tabler-plus">
              เพิ่มข้อมูล
            </VBtn>
          </RouterLink>
        </div>
      </VCardText>

      <VDivider />

      <!-- SECTION datatable -->
      <VDataTableServer
        v-model:items-per-page="itemsPerPage"
        v-model:model-value="selectedRows"
        v-model:page="page"
        :items="contracts"
        item-value="id"
        :items-length="totalContracts"
        :headers="headers"
        class="compact-table"
        show-select
        @update:options="updateOptions"
      >
        <!-- Status -->
        <template #item.status="{ item }">
          <VTooltip>
            <template #activator="{ props }">
              <VAvatar
                v-bind="props"
                :color="resolveContractStatusVariant(item.status)?.variant"
                variant="tonal"
                size="30"
              >
                <VIcon
                  :icon="resolveContractStatusVariant(item.status)?.icon"
                  size="18"
                />
              </VAvatar>
            </template>
            <p class="mb-0">
              {{ item.statusText || item.status }}
            </p>
          </VTooltip>
        </template>

        <!-- Details + Number (merged) -->
        <template #item.details="{ item }">
          <div class="d-flex flex-column">
            <span class="d-block text-body-2 text-disabled">{{ item.number || '-' }}</span>
            <RouterLink
              :to="{ name: 'contracts-view-id', params: { id: item.id } }"
              class="d-block font-weight-medium text-high-emphasis text-truncate text-link"
            >
              {{ item.details || '-' }}
            </RouterLink>
          </div>
        </template>

        <!-- Bid Winner -->
        <template #item.bidWinnerId="{ item }">
          <span v-if="item.bidWinnerName">{{ item.bidWinnerName }}</span>
          <span v-else-if="item.bidWinnerId">{{ item.bidWinnerId }}</span>
          <span v-else>-</span>
        </template>

        <!-- Contract Type -->
        <template #item.contractTypeId="{ item }">
          <span>{{ item.contractTypeName || item.contractTypeId || '-' }}</span>
        </template>

        <!-- Start Date -->
        <template #item.startDate="{ item }">
          <span>{{ formatDate(item.startDate) }}</span>
        </template>

        <!-- End Date -->
        <template #item.endDate="{ item }">
          <span>{{ formatDate(item.endDate) }}</span>
        </template>

        <!-- Budget -->
        <template #item.budget="{ item }">
          <span class="font-weight-medium">{{ item.budget != null ? formatCurrency(item.budget) : '-' }}</span>
        </template>

        <!-- Budget Diff -->
        <template #item.budgetDiff="{ item }">
          <VChip
            v-if="item.budgetDiff != null"
            :color="item.budgetDiff > 0 ? 'success' : item.budgetDiff < 0 ? 'error' : 'secondary'"
            size="small"
            label
          >
            {{ formatCurrency(item.budgetDiff) }}
          </VChip>
          <span v-else>-</span>
        </template>

        <!-- Last Modified -->
        <template #item.lastModified="{ item }">
          <span class="text-body-2">{{ formatDate(item.lastModified) }}</span>
        </template>

        <!-- Actions -->
        <template #item.actions="{ item }">
          <RouterLink :to="{ name: 'contracts-view-id', params: { id: item.id } }">
            <IconBtn>
              <VIcon icon="tabler-eye" />
            </IconBtn>
          </RouterLink>

          <VBtn
            icon
            variant="text"
            color="medium-emphasis"
          >
            <VIcon icon="tabler-dots-vertical" />
            <VMenu activator="parent">
              <VList>
                <VListItem :to="{ name: 'contracts-view-id', params: { id: item.id } }">
                  <template #prepend>
                    <VIcon icon="tabler-eye" />
                  </template>
                  <VListItemTitle>แสดง</VListItemTitle>
                </VListItem>

                <VListItem @click="confirmDeleteContract(item)">
                  <template #prepend>
                    <VIcon icon="tabler-trash" />
                  </template>
                  <VListItemTitle>ลบ</VListItemTitle>
                </VListItem>
              </VList>
            </VMenu>
          </VBtn>
        </template>

        <!-- pagination -->
        <template #bottom>
          <TablePagination
            v-model:page="page"
            :items-per-page="itemsPerPage"
            :total-items="totalContracts"
          />
        </template>
      </VDataTableServer>

      <!-- 👉 Delete Confirmation Dialog -->
      <ConfirmProgressDialog
        ref="progressDialogRef"
        v-model:model-value="isConfirmProgressDialogVisible"
        confirm-title="โปรดยืนยันการลบข้อมูล"
        confirm-message="ข้อมูลสัญญาที่เกี่ยวข้องจะถูกลบทั้งหมด"
        progress-message="กำลังประมวลผล, โปรดรอสักครู่..."
        success-title="ลบข้อมูลสำเร็จ!"
        success-message="ลบข้อมูลสัญญาสำเร็จ"
        failure-title="ลบข้อมูลไม่สำเร็จ"
        failure-message="ลบข้อมูลไม่สำเร็จ, โปรดตรวจสอบ!"
        :failure-data="progressDialogFailureDescription"
        @confirm="deleteContract(selectedContract.id)"
        @retry="deleteContract(selectedContract.id)"
        @cancel="cancelDeleteContract"
      />
    </VCard>
  </section>
</template>

<style lang="scss" scoped>
.compact-table {
  :deep(table) {
    table-layout: fixed;
    width: 100%;
  }

  :deep(th),
  :deep(td) {
    font-size: 0.8125rem !important;
    padding-block: 0.5rem !important;
    padding-inline: 0.5rem !important;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
  }

  :deep(th) {
    font-size: 0.75rem !important;
    font-weight: 600;
  }
}
</style>
