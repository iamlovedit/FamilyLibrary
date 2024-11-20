import { useAppSelector, useAppDispatch } from '@/stores'
import { decrement, increment } from '@/stores/modules/counter'

import { Button } from '@nextui-org/react'

function Home() {
  return <Button color="primary">Press me!</Button>
}

export default Home
