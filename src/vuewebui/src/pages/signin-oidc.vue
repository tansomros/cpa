<template>
  <div>
    <VCard
      class="mb-6"
      title="Processing... 🚀"
    >
      <VCardText>กำลังเข้าสู่ระบบ, โปรดรอสักครู่...</VCardText>
      <VCardText>
        ระบบกำลังนำท่านไปยังหน้าหลัก
      </VCardText>
    </VCard>
  </div>
</template>

<script setup>
import { userService } from '@/plugins/auth'
import { useAbility } from '@casl/vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const ability = useAbility()

userService.completeSignin()
  .then(async user => {

    if (user && user.access_token) {
      localStorage.removeItem("userData")
      localStorage.removeItem("accessToken")
      useCookie('accessToken').value = null
      useCookie('userAbilityRules').value = null

      localStorage.setItem("userData", JSON.stringify(user))
      localStorage.setItem("accessToken", user.access_token)
      useCookie('userData').value = user
      useCookie('accessToken').value = user.access_token

      const userAbilityRules = [{
        action: 'manage',
        subject: 'all',
      }]

      ability.update(userAbilityRules)
      useCookie('userAbilityRules').value = { userAbilityRules }

      router.replace(router.currentRoute.value.query.to || '/')
    } else {

      userService.signin()
    }
  })
  .catch(error => {
    console.error("Sign-in callback failed: ", error)
    userService.signin()
  })
</script>
