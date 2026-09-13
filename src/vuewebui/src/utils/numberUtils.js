// eslint-disable-next-line import/extensions
export const formatNumber = (value, fraction = 4) => {
  if (isNaN(value)) return '-'
  return new Intl.NumberFormat('th-TH', {
    minimumFractionDigits: fraction,
    maximumFractionDigits: fraction
  }).format(value)
}

export const formatCurrency = (value) => {
  if (isNaN(value)) return '-'
  return new Intl.NumberFormat('th-TH', {
    style: 'currency',
    currency: 'THB',
    minimumFractionDigits: 2
  }).format(value)
}

export const formatPercent = (value) => {
  if (isNaN(value)) return '-'
  return new Intl.NumberFormat('th-TH', {
    style: 'percent',
    minimumFractionDigits: 2
  }).format(value / 100)
}
