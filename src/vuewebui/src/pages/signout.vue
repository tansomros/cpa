<template>
  <div>
    <VCard class="mb-6" title="Kick start your project 🚀">
      <VCardText>Signin out, please wait...</VCardText>
      <VCardText>
        You will be redirect to signin page
      </VCardText>
    </VCard>
  </div>
</template>

<script setup>
import { userService } from '@/plugins/auth';

await userService.signout()
</script>
