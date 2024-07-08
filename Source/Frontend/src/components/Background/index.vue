<template>
  <canvas id="container" ref="container"> </canvas>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import * as THREE from 'three'

const container = ref<HTMLCanvasElement | null>(null)

onMounted(() => {
  if (container.value) {
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
    animate()
  }
})
</script>

<style scoped>
#container {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  z-index: -1;
}
</style>
