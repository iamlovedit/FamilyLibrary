import { createSlice, PayloadAction, Slice } from '@reduxjs/toolkit'
import type { RootState } from '..'
export interface CounterState {
  value: number
  status: 'idle' | 'loading' | 'failed'
}

const initialState: CounterState = {
  value: 0,
  status: 'idle'
}

export const counterSlice: Slice = createSlice({
  name: 'counter',
  initialState,
  reducers: {
    increment: (state) => {
      // Redux Toolkit 允许我们在 reducers 写 "可变" 逻辑。它
      // 并不是真正的改变状态值，因为它使用了 Immer 库
      // 可以检测到“草稿状态“ 的变化并且基于这些变化生产全新的
      // 不可变的状态
      state.value += 1
    },
    decrement: (state) => {
      state.value -= 1
    },
    incrementByAmount: (state: CounterState, action: PayloadAction<number>) => {
      state.value += action.payload
    }
  }
})
// 每个 case reducer 函数会生成对应的 Action creators
export const { increment, decrement, incrementByAmount } = counterSlice.actions
export const selectCount = (state: RootState) => state.counter.value
export default counterSlice.reducer
