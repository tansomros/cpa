<script setup>
import { toBuddhistYear } from '@/utils/dateUtils'
import { formatNumber, formatCurrency } from '@/utils/numberUtils'

// eslint-disable-next-line import/extensions
import moment from 'moment/min/moment-with-locales.js'

moment.locale('th')
const route = useRoute('contracts-view-id')
const router = useRouter()

const { data: contractsData } = await useApi(`contracts/${route.params.id}`)

const contractNumber = encodeURIComponent(contractsData.value.number)
const { data: invHosxpData } = await useHosxpApi(`/Inventory?ContractNumber=${contractNumber}`, { method: 'GET' })
const inventory = computed(() => invHosxpData.value)

const lastInvHosxpData = computed(() => {
  if (!invHosxpData.value || invHosxpData.value.length === 0) return null
  const last = invHosxpData.value.at(-1)
  return {
    sumPOAmount: last.sumPOAmount,
    stockVendorName: last.stockVendorName,
    contractBeginDate: last.contractBeginDate,
    contractEndDate: last.contractEndDate,
  }
})

// 👉 Computed totals
const totalPOAmount = computed(() => lastInvHosxpData.value?.sumPOAmount ?? 0)
const budgetAmount = computed(() => contractsData.value?.budget ?? 0)
const remainingAmount = computed(() => budgetAmount.value - totalPOAmount.value)

// 👉 Print handler
const printContract = () => {
  window.print()
}
</script>

<template>
  <section v-if="contractsData">
    <VRow>
      <!-- 👉 Contract Preview Card (Main Content) -->
      <VCol
        cols="12"
        md="9"
      >
        <VCard>
          <!-- Header -->
          <VCardText class="d-flex flex-wrap justify-space-between flex-column flex-sm-row print-row">
            <!-- Left: Contract Info -->
            <div class="mb-4">
              <div class="d-flex align-center mb-6">
                <VAvatar
                  color="primary"
                  variant="tonal"
                  rounded
                  size="42"
                  class="me-3"
                >
                  <VIcon
                    icon="tabler-file-dollar"
                    size="24"
                  />
                </VAvatar>
                <h5 class="text-h5 font-weight-bold text-primary">
                  รายละเอียดสัญญา
                </h5>
              </div>

              <p class="mb-1">
                <span class="text-body-2 text-disabled me-2">เลขที่สัญญา:</span>
                <span class="font-weight-medium text-high-emphasis">{{ contractsData.number }}</span>
              </p>
              <p class="mb-1">
                <span class="text-body-2 text-disabled me-2">รายการ:</span>
                <span class="font-weight-medium text-high-emphasis">{{ contractsData.details }}</span>
              </p>
            </div>

            <!-- Right: Date Info -->
            <div class="mb-4">
              <h6 class="text-h6 font-weight-medium mb-4">
                ข้อมูลสัญญา
              </h6>

              <table>
                <tbody>
                  <tr>
                    <td class="pe-6 text-body-2 text-disabled">
                      วันที่เริ่มสัญญา:
                    </td>
                    <td class="font-weight-medium">
                      <template v-if="lastInvHosxpData?.contractBeginDate">
                        {{ toBuddhistYear(moment(lastInvHosxpData.contractBeginDate), "D MMMM YYYY") }}
                      </template>
                      <template v-else>
                        -
                      </template>
                    </td>
                  </tr>
                  <tr>
                    <td class="pe-6 text-body-2 text-disabled">
                      วันที่สิ้นสุดสัญญา:
                    </td>
                    <td class="font-weight-medium">
                      <template v-if="lastInvHosxpData?.contractEndDate">
                        {{ toBuddhistYear(moment(lastInvHosxpData.contractEndDate), "D MMMM YYYY") }}
                      </template>
                      <template v-else>
                        -
                      </template>
                    </td>
                  </tr>
                  <tr>
                    <td class="pe-6 text-body-2 text-disabled">
                      ประเภทสัญญา:
                    </td>
                    <td class="font-weight-medium">
                      {{ contractsData.contractTypeName || contractsData.contractTypeId || '-' }}
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </VCardText>

          <VDivider />

          <!-- Vendor Info -->
          <VCardText class="d-flex flex-wrap justify-space-between flex-column flex-sm-row print-row">
            <div class="mb-4">
             <h6 class="text-h6 font-weight-medium mb-3">
                <VIcon
                  icon="tabler-building"
                  size="20"
                  class="me-1"
                />
                บริษัท/ห้าง/ร้าน
              </h6>
              <p class="mb-1 font-weight-medium text-high-emphasis">
                {{ lastInvHosxpData?.stockVendorName || contractsData.bidWinnerName || '-' }}
              </p>
            </div>

            <div class="mb-4">
              <h6 class="text-h6 font-weight-medium mb-3">
                <VIcon
                  icon="tabler-cash"
                  size="20"
                  class="me-1"
                />
                วงเงินตามสัญญา
              </h6>
              <p class="mb-0 text-h5 font-weight-bold text-primary">
                {{ formatCurrency(budgetAmount) }}
              </p>
            </div>
          </VCardText>

          <VDivider />

          <!-- 👉 Purchase Order Items Table -->
          <VTable class="invoice-preview-table">
            <thead>
              <tr>
                <th class="text-center" style="width: 60px;">
                  ลำดับ
                </th>
                <th>เลขที่สั่งซื้อ</th>
                <th>ว/ด/ปี</th>
                <th class="text-end">
                  จำนวนเงินสั่งซื้อ
                </th>
                <th class="text-end">
                  จำนวนเงินตรวจรับ
                </th>
                <th class="text-end">
                  จำนวนเงินหยุดจัดส่ง
                </th>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="(item, index) in inventory"
                :key="index"
              >
                <td class="text-center">
                  {{ index + 1 }}
                </td>
                <td>{{ item.stockPoNumber }}</td>
                <td>{{ toBuddhistYear(moment(item.stockPoDate), "D MMM YYYY") }}</td>
                <td class="text-end">
                  {{ formatNumber(item.stockPoAmount ?? 0) }}
                </td>
                <td class="text-end">
                  {{ formatNumber(item.stockDeliverAmount ?? 0) }}
                </td>
                <td class="text-end">
                  {{ formatNumber(item.stockPODiff ?? 0) }}
                </td>
              </tr>

              <!-- Empty state -->
              <tr v-if="!inventory || inventory.length === 0">
                <td
                  colspan="6"
                  class="text-center text-disabled py-6"
                >
                  ไม่พบข้อมูลการสั่งซื้อ
                </td>
              </tr>
            </tbody>
          </VTable>

          <VDivider />

          <!-- 👉 Totals Summary -->
          <VCardText class="d-flex justify-end">
            <div style="min-inline-size: 280px;">
              <table class="w-100">
                <tbody>
                  <tr>
                    <td class="text-body-2 text-disabled pe-6 pb-2">
                      ยอดรวมสั่งซื้อ:
                    </td>
                    <td class="text-end font-weight-medium pb-2">
                      {{ formatNumber(totalPOAmount) }}
                    </td>
                  </tr>
                  <tr>
                    <td class="text-body-2 text-disabled pe-6 pb-2">
                      วงเงินตามสัญญา:
                    </td>
                    <td class="text-end font-weight-medium pb-2">
                      {{ formatNumber(budgetAmount) }}
                    </td>
                  </tr>

                  <tr>
                    <td colspan="2">
                      <VDivider class="my-2" />
                    </td>
                  </tr>

                  <tr>
                    <td class="text-body-1 font-weight-medium pe-6">
                      ยอดคงเหลือ:
                    </td>
                    <td class="text-end">
                      <span
                        class="text-h6 font-weight-bold"
                        :class="remainingAmount >= 0 ? 'text-success' : 'text-error'"
                      >
                        {{ formatNumber(remainingAmount) }}
                      </span>
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
          </VCardText>

          <VDivider />

          <!-- Note -->
          <VCardText>
            <p class="text-body-2 text-disabled mb-0">
              <strong>หมายเหตุ:</strong> ข้อมูลนี้เป็นข้อมูลสรุปจากระบบจัดซื้อ โปรดตรวจสอบความถูกต้องกับเอกสารต้นฉบับ
            </p>
          </VCardText>
        </VCard>
      </VCol>

      <!-- 👉 Action Sidebar -->
      <VCol
        cols="12"
        md="3"
      >
        <VCard>
          <VCardText class="pa-4">
            <!-- Back to List -->
            <VBtn
              block
              size="small"
              variant="tonal"
              color="secondary"
              class="mb-2"
              prepend-icon="tabler-arrow-left"
              :to="{ name: 'contracts-list' }"
            >
              กลับรายการ
            </VBtn>

            <!-- Print -->
            <VBtn
              block
              size="small"
              color="primary"
              class="mb-2"
              prepend-icon="tabler-printer"
              @click="printContract"
            >
              พิมพ์
            </VBtn>

            <!-- Download PDF -->
            <VBtn
              block
              size="small"
              variant="tonal"
              color="success"
              prepend-icon="tabler-download"
            >
              ดาวน์โหลด PDF
            </VBtn>
          </VCardText>
        </VCard>

        <!-- 👉 Summary Quick Card -->
        <VCard class="mt-4">
          <VCardItem class="pa-4 pb-2">
            <VCardTitle class="text-body-2 font-weight-medium">
              <VIcon
                icon="tabler-info-circle"
                size="18"
                class="me-1"
              />
              สรุป
            </VCardTitle>
          </VCardItem>

          <VCardText>
            <VList density="compact">
              <VListItem>
                <template #prepend>
                  <VIcon
                    icon="tabler-receipt"
                    color="primary"
                    size="20"
                  />
                </template>
                <VListItemTitle class="text-body-2">
                  จำนวนรายการสั่งซื้อ
                </VListItemTitle>
                <template #append>
                  <span class="font-weight-medium">{{ inventory?.length ?? 0 }} รายการ</span>
                </template>
              </VListItem>

              <VListItem>
                <template #prepend>
                  <VIcon
                    icon="tabler-cash"
                    color="success"
                    size="20"
                  />
                </template>
                <VListItemTitle class="text-body-2">
                  วงเงินตามสัญญา
                </VListItemTitle>
                <template #append>
                  <span class="font-weight-medium">{{ formatCurrency(budgetAmount) }}</span>
                </template>
              </VListItem>

              <VListItem>
                <template #prepend>
                  <VIcon
                    icon="tabler-chart-pie"
                    :color="remainingAmount >= 0 ? 'warning' : 'error'"
                    size="20"
                  />
                </template>
                <VListItemTitle class="text-body-2">
                  ยอดคงเหลือ
                </VListItemTitle>
                <template #append>
                  <span
                    class="font-weight-bold"
                    :class="remainingAmount >= 0 ? 'text-success' : 'text-error'"
                  >
                    {{ formatCurrency(remainingAmount) }}
                  </span>
                </template>
              </VListItem>
            </VList>
          </VCardText>
        </VCard>
      </VCol>
    </VRow>
  </section>

  <!-- Empty State -->
  <VAlert
    v-else
    type="error"
    variant="tonal"
  >
    ไม่พบข้อมูล, โปรดเลือกข้อมูลที่ต้องการแสดงในหน้า "รายการ"
  </VAlert>
</template>

<style lang="scss">
.invoice-preview-table {
  th {
    font-size: 0.8125rem !important;
    font-weight: 600;
    background-color: rgba(var(--v-theme-on-surface), 0.04);
  }

  td {
    font-size: 0.8125rem !important;
  }
}

@media print {
  .layout-menu,
  .layout-navbar,
  .layout-footer {
    display: none !important;
  }

  .layout-page {
    padding: 0 !important;
    margin: 0 !important;
  }

  .v-col-md-3 {
    display: none !important;
  }

  .v-col-md-9 {
    max-inline-size: 100% !important;
    flex: 0 0 100% !important;
  }

  .v-card {
    box-shadow: none !important;
  }
}
</style>
