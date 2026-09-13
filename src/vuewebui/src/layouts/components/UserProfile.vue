<script setup>
import avatar1 from '@images/avatars/avatar-1.png'

const userJson = localStorage.getItem("userData")
const user = JSON.parse(userJson)

let userFullName = 'ไม่มีข้อมูล'
let employeeId = 'ไม่มีข้อมูล'

if (user != null) {
  userFullName = user.profile?.displayName ?? user.userFullName ?? user.displayName ?? 'ไม่มีข้อมูล'
  employeeId = user.profile?.Id ?? user.id ?? 'ไม่มีข้อมูล'
}
</script>

<template>
  <VBadge dot location="bottom right" offset-x="3" offset-y="3" bordered color="success">
    <VAvatar class="cursor-pointer" color="primary" variant="tonal">
      <VImg :src="avatar1" />

      <!-- SECTION Menu -->
      <VMenu activator="parent" width="230" location="bottom end" offset="14px">
        <VList>
          <!-- 👉 User Avatar & Name -->
          <VListItem>
            <template #prepend>
              <VListItemAction start>
                <VBadge dot location="bottom right" offset-x="3" offset-y="3" color="success">
                  <VAvatar color="primary" variant="tonal">
                    <VImg :src="avatar1" />
                  </VAvatar>
                </VBadge>
              </VListItemAction>
            </template>

            <VListItemTitle class="font-weight-semibold">
              {{ userFullName }}
            </VListItemTitle>
            <VListItemSubtitle>{{ employeeId }}</VListItemSubtitle>
          </VListItem>

          <VDivider class="my-2" />

          <!-- 👉 Logout -->
          <VListItem to="/signout">
            <template #prepend>
              <VIcon class="me-2" icon="tabler-logout" size="22" />
            </template>

            <VListItemTitle>ออกจากระบบ</VListItemTitle>
          </VListItem>
        </VList>
      </VMenu>
      <!-- !SECTION -->
    </VAvatar>
  </VBadge>
</template>
