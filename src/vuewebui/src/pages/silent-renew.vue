<script setup>
window.opener.postMessage("oidc-silent-renew-complete", window.origin)
</script>

<template>
  <div>
    <VCard title="Processing Silent Renewal">
      <VCardText>
        Processing Silent Renewal...
      </VCardText>
    </VCard>
  </div>
</template>
