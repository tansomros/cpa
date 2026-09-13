import { createMongoAbility } from '@casl/ability'
import { abilitiesPlugin } from '@casl/vue'

export default function (app) {
  const userAbilityRules = useCookie('userAbilityRules')
  
  // Ensure rules is always an array to prevent crash if cookie is malformed
  let rules = userAbilityRules.value ?? []
  if (rules && !Array.isArray(rules)) {
    rules = rules.userAbilityRules || []
  }

  const initialAbility = createMongoAbility(rules)

  app.use(abilitiesPlugin, initialAbility, {
    useGlobalProperties: true,
  })
}
