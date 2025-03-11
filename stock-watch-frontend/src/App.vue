<template>
  <Navbar />
  <router-view></router-view>
</template>

<script>
import Navbar from "@/components/AppNavbar.vue";

export default {
  components: { Navbar },
};
</script>
