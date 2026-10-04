<script setup>
import PharmacyLookupList from '@/components/pharmacy-lookups/PharmacyLookupList.vue'
</script>


<template>
  <PharmacyLookupList
    title="กลุ่มร้านยา"
    api-path="/pharmacy-groups"
  />
</template>
