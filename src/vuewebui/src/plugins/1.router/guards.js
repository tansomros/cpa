import { useAbility } from "@casl/vue"

export const setupGuards = router => {
  router.beforeEach(async to => {
    // allow public routes like login
    if(to.meta.public) return 

    const ability = useAbility()
    const user = getUserFromStorage()

    if(!user) {
      clearUserData()
      // redirect to local login page instead of OIDC
      return { name: 'login', query: { to: to.path !== '/' ? to.path : undefined } }
    }

    updateAbility(ability)
  })
}

function getUserFromStorage() {
  const userJson = localStorage.getItem("userData")  
  
  return userJson ? JSON.parse(userJson) : null
}

function clearUserData() {
  localStorage.removeItem("userData")
  localStorage.removeItem("accessToken")
  useCookie("userData").value = null
  useCookie("accessToken").value = null
  useCookie("userAbilityRules").value = null
}

function updateAbility(ability) {
  const userAbilityRules = [{ action: 'manage', subject: 'all' }]

  ability.update(userAbilityRules)
  useCookie("userAbilityRules").value = userAbilityRules
}
