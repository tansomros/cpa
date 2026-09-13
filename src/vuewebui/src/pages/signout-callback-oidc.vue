<template>
  <div>
    <VCard class="mb-6" title="Kick start your project 🚀">
      <VCardText>Signing out, please wait...</VCardText>
      <VCardText>
        You will be redirect to the app home page
      </VCardText>
    </VCard>
  </div>
</template>

<script setup>
import { userService } from '@/plugins/auth';

const ability = useAbility()

userService.completeSignout().then(async user => {
  localStorage.removeItem("userData")
  localStorage.removeItem("accessToken")
  useCookie('accessToken').value = null
  useCookie('userAbilityRules').value = null
  ability.update([])

  await userService.signin()
})
</script>
