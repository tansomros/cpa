<script setup>
import PharmacyLookupList from '@/components/pharmacy-lookups/PharmacyLookupList.vue'
</script>

<template>
  <PharmacyLookupList
    title="ประเภทร้านยา"
    api-path="/pharmacy-types"
  />
</template>
