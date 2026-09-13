import { defineStore } from 'pinia'
import { $api } from '@/utils/api'

export const useLookupStore = defineStore('lookup', {
  state: () => ({
    cache: {},
  }),

  actions: {
    /**
     * Fetches and caches lookup options for a given category.
     * @param {string} category - e.g. 'exam-results', 'checkup-statuses'
     * @param {string|null} lang - optional language code e.g. 'en', 'th'
     * @returns {Promise<Array<{value: string, displayName: string}>>}
     */
    async getOptions(category, lang = null) {
      const key = `${category}:${lang || 'default'}`

      if (!this.cache[key]) {
        const query = lang ? `?lang=${lang}` : ''

        this.cache[key] = await $api(`/Options/${category}${query}`)
      }

      return this.cache[key]
    },

    /**
     * Fetches all available lookup category names.
     * @returns {Promise<string[]>}
     */
    async getCategories() {
      if (!this.cache._categories) {
        this.cache._categories = await $api('/Options')
      }

      return this.cache._categories
    },

    clearCache() {
      this.cache = {}
    },
  },
})
