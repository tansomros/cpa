<script setup>
import { $api } from '@/utils/api'
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const username = ref('')
const password = ref('')
const isPasswordVisible = ref(false)
const errorMessage = ref('')
const isLoading = ref(false)

const login = async () => {
  if (!username.value || !password.value) {
    errorMessage.value = 'Please enter username and password'
    return
  }

  try {
    isLoading.value = true
    errorMessage.value = ''

    const data = await $api('/api/auth/login', {
      method: 'POST',
      body: {
        username: username.value,
        password: password.value
      }
    })

    if (data.accessToken) {
      // Set accessToken
      const token = useCookie('accessToken')
      token.value = data.accessToken
      localStorage.setItem('accessToken', token.value)
      
      // Set userData for router guards
      const userData = {
        id: data.userId,
        username: data.username,
        displayName: data.displayName,
        role: data.role || 'admin',
      }
      const userDataCookie = useCookie('userData')
      userDataCookie.value = userData
      localStorage.setItem('userData', JSON.stringify(userData))
    }
    
    // Redirect to the originally requested route or dashboards
    const routeQuery = router.currentRoute.value.query
    router.push(routeQuery.to ? String(routeQuery.to) : '/dashboards')
  } catch (error) {
    if (error.response?.status === 401) {
      errorMessage.value = 'Invalid username or password'
    } else {
      errorMessage.value = error.response?._data?.title || error.response?._data?.message || error.message || 'Login failed. Please try again.'
    }
  } finally {
    isLoading.value = false
  }
}
</script>

<template>
  <div class="auth-wrapper d-flex align-center justify-center pa-4">
    <VCard class="auth-card pa-4 pt-7" max-width="448">
      <VCardItem class="justify-center">
        <VCardTitle class="font-weight-bold text-h4 py-1 text-primary">
          Kondongpu
        </VCardTitle>
      </VCardItem>

      <VCardText class="pt-2">
        <h5 class="text-h5 font-weight-semibold mb-1">
          Welcome to Kondongpu! 👋🏻
        </h5>
        <p class="mb-0">
          Please sign-in to your account and start the adventure
        </p>
      </VCardText>

      <VCardText>
        <VAlert v-if="errorMessage" type="error" variant="tonal" class="mb-4">
          {{ errorMessage }}
        </VAlert>

        <VForm @submit.prevent="login">
          <VRow>
            <!-- username -->
            <VCol cols="12">
              <VTextField v-model="username" label="Username" type="text" placeholder="admin" :error="!!errorMessage" />
            </VCol>

            <!-- password -->
            <VCol cols="12">
              <VTextField v-model="password" label="Password" placeholder="············"
                :type="isPasswordVisible ? 'text' : 'password'"
                :append-inner-icon="isPasswordVisible ? 'tabler-eye-off' : 'tabler-eye'"
                @click:append-inner="isPasswordVisible = !isPasswordVisible" :error="!!errorMessage" />

              <!-- remember me checkbox -->
              <div class="d-flex align-center justify-space-between flex-wrap mt-1 mb-4">
                <VCheckbox label="Remember me" />
                <a class="text-primary text-decoration-none" href="javascript:void(0)">Forgot Password?</a>
              </div>

              <!-- login button -->
              <VBtn block type="submit" color="primary" :loading="isLoading" :disabled="isLoading">
                Login
              </VBtn>
            </VCol>
          </VRow>
        </VForm>
      </VCardText>
    </VCard>
  </div>
</template>

<route lang="yaml">
meta:
  layout: blank
  public: true
</route>

<style scoped>
.auth-wrapper {
  background-color: rgb(var(--v-theme-background));
  min-block-size: 100vh;
}

.auth-card {
  inline-size: 100%;
}
</style>
