import { useEffect, useState } from 'react'
import { useLoaderData } from 'react-router-dom'

function Home() {
  const [mounted, setMounted] = useState(false)
  const hello = useLoaderData()

  useEffect(() => {
    setMounted(true)
  }, [])

  if (!mounted) {
    return null
  }

  return <>{hello}</>
}

export default Home
