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
          <n-tab-pane name="3d" tab="三维视图">
            <canvas ref="container" class="h-full w-full" />
          </n-tab-pane>
          <n-tab-pane name="covers" tab="封面">
            <n-carousel show-arrow>
              <img
                v-for="cover in familyRef?.covers"
                :key="cover"
                class="carousel-img"
                :src="cover"
              />
            </n-carousel>
          </n-tab-pane>
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
import { ref, onMounted } from 'vue'
import { useMessage, useLoadingBar } from 'naive-ui'
import { useRoute, useRouter } from 'vue-router'
import { type FamilyDetail, getFamilyDetailPromise } from '@/api/family'
import { StarOutline, CloudDownloadOutline } from '@vicons/ionicons5'
import { MdThumbsUp } from '@vicons/ionicons4'
import * as THREE from 'three'

const router = useRouter()
const currentRoute = useRoute()
const message = useMessage()
const loadingBar = useLoadingBar()
const familyRef = ref<FamilyDetail>()
const container = ref<HTMLCanvasElement | null>(null)

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

onMounted(() => {
  if (container.value) {
    console.log(111)
    const renderer: THREE.WebGLRenderer = new THREE.WebGLRenderer({
      antialias: true,
      canvas: container.value
    })
    renderer.outputColorSpace = THREE.SRGBColorSpace
    renderer.setSize(container.value.offsetWidth, container.value.offsetHeight, true)
    renderer.shadowMap.enabled = true
    renderer.setPixelRatio(window.devicePixelRatio)
    const scene = new THREE.Scene()
    const camera = new THREE.PerspectiveCamera(
      75,
      window.innerWidth / window.innerHeight,
      0.1,
      1000
    )
    camera.position.z = 5
    const starGeometry = new THREE.SphereGeometry(1, 32, 32)
    const starMaterial = new THREE.MeshBasicMaterial({ color: 0xffffff })
    const stars = new THREE.Group()

    for (let i = 0; i < 20000; i++) {
      const star = new THREE.Mesh(starGeometry, starMaterial)

      star.position.x = THREE.MathUtils.randFloatSpread(2000)
      star.position.y = THREE.MathUtils.randFloatSpread(2000)
      star.position.z = THREE.MathUtils.randFloatSpread(2000)

      const scale = THREE.MathUtils.randFloat(0.1, 1)
      star.scale.set(scale, scale, scale)

      stars.add(star)
    }

    scene.add(stars)
    // eslint-disable-next-line no-inner-declarations
    function animate() {
      requestAnimationFrame(animate)
      stars.rotation.y += 0.001
      renderer.render(scene, camera)
    }
    // eslint-disable-next-line no-inner-declarations
    function handleWindowResize() {
      const width = container.value?.offsetWidth as number
      const height = container.value?.offsetHeight as number

      camera.aspect = width / height
      camera.updateProjectionMatrix()

      renderer.setSize(width, height)
    }

    animate()
    handleWindowResize()
  }
})
</script>
<style scoped>
.carousel-img {
  width: 100%;
  height: 440px;
  object-fit: cover;
}
</style>
