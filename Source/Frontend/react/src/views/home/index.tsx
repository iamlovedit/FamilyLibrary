import { useEffect, useState } from 'react'

function Home() {
  const [mounted, setMounted] = useState(false)
  useEffect(() => {
    setMounted(true)
  }, [])

  if (!mounted) {
    return null
  }

  return <>{'Home view'}</>
}

export default Home
