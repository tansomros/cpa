export default [
  {
    title: 'ตั้งค่าสัญญา',
    icon: { icon: 'tabler-settings' },
    children: [
      { title: 'ประเภทสัญญา', to: 'contract-types-list' },
      { title: 'ประเภทหลักประกันสัญญา', to: 'contract-bonds-list' },
      { title: 'ประเภทคณะกรรมการ', to: 'contract-committee-types-list' },
      { title: 'ประเภทกำหนดราคากลาง', to: 'reference-price-types-list' },
    ],
  },
]
