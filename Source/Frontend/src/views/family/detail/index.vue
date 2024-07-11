<template>
  <n-flex vertical>
    <n-card>
      <n-page-header title="族详情" @back="handleBack" class="w-full">
        <n-flex justify="space-between">
          <n-flex>
            <n-statistic label="浏览" :value="`${familyRef?.views} 次`" />
            <n-statistic label="下载" :value="`${familyRef?.downloads} 次`" />
            <n-statistic label="点赞" :value="`${familyRef?.stars} 次`" />
          </n-flex>
          <n-flex>
            <n-button :bordered="false" ghost>
              <template #icon>
                <n-icon>
                  <StarOutline />
                </n-icon>
              </template>
            </n-button>
            <n-button :bordered="false" ghost>
              <template #icon>
                <n-icon>
                  <MdThumbsUp />
                </n-icon>
              </template>
            </n-button>
            <n-button :bordered="false" ghost>
              <template #icon>
                <n-icon>
                  <CloudDownloadOutline />
                </n-icon>
              </template>
            </n-button>
          </n-flex>
        </n-flex>
      </n-page-header>
    </n-card>
    <n-flex class="flex-1 flex-nowrap gap-6">
      <n-card class="flex-1">
        <n-tabs type="line" animated>
          <n-tab-pane name="covers" tab="封面">
            <n-carousel show-arrow>
              <img
                class="carousel-img"
                src="https://naive-ui.oss-cn-beijing.aliyuncs.com/carousel-img/carousel1.jpeg"
              />
              <img
                class="carousel-img"
                src="https://naive-ui.oss-cn-beijing.aliyuncs.com/carousel-img/carousel2.jpeg"
              />
              <img
                class="carousel-img"
                src="https://naive-ui.oss-cn-beijing.aliyuncs.com/carousel-img/carousel3.jpeg"
              />
              <img
                class="carousel-img"
                src="https://naive-ui.oss-cn-beijing.aliyuncs.com/carousel-img/carousel4.jpeg"
              />
            </n-carousel>
          </n-tab-pane>
          <n-tab-pane name="3d" tab="三维视图"> </n-tab-pane>
        </n-tabs>
      </n-card>
      <n-card class="w-500px">
        <n-collapse>
          <n-collapse-item title="青铜" name="1">
            <div>可以</div>
          </n-collapse-item>
          <n-collapse-item title="白银" name="2">
            <div>很好</div>
          </n-collapse-item>
          <n-collapse-item title="黄金" name="3">
            <div>真棒</div>
          </n-collapse-item>
        </n-collapse>
      </n-card>
    </n-flex>
  </n-flex>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useMessage, useLoadingBar } from 'naive-ui'
import { useRoute, useRouter } from 'vue-router'
import { type FamilyDetail, getFamilyDetailPromise } from '@/api/family'
import { StarOutline, CloudDownloadOutline } from '@vicons/ionicons5'
import { MdThumbsUp } from '@vicons/ionicons4'

const router = useRouter()
const currentRoute = useRoute()
const message = useMessage()
const loadingBar = useLoadingBar()
const familyRef = ref<FamilyDetail>()

async function getFamilyDetailAsync(id: string) {
  try {
    loadingBar.start()
    const httpMessage = await getFamilyDetailPromise(id)
    if (httpMessage.succeed) {
      familyRef.value = httpMessage.response
    } else {
      throw new Error(httpMessage.message)
    }
  } catch (error: any) {
    message.error(error.message)
  } finally {
    loadingBar.finish()
  }
}

function handleBack() {
  router.back()
}
getFamilyDetailAsync(currentRoute.params.id as string)
</script>
<style scoped>
.carousel-img {
  width: 100%;
  height: 440px;
  object-fit: cover;
}
</style>
