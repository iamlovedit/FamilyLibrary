import { useAppSelector, useAppDispatch } from '@/stores'
import { decrement, increment } from '@/stores/modules/counter'


function Home() {

    const count = useAppSelector((state: any) => state.counter.value)
    const dispatch = useAppDispatch()

    return (
        <div>
            <button
                aria-label="Increment value"
                onClick={() => dispatch(increment())}
            >
                Increment
            </button>
            <span>{count}</span>
            <button
                aria-label="Decrement value"
                onClick={() => dispatch(decrement())}
            >
                Decrement
            </button>
        </div>
    )
}

export default Home