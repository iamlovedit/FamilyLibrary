import { configureStore, ThunkAction, Action } from '@reduxjs/toolkit'
import counterReducer from './modules/counter'
import { useDispatch, useSelector } from 'react-redux'

const store = configureStore({
  reducer: {
    counter: counterReducer
  }
})

export type RootState = ReturnType<typeof store.getState>
export type AppDispatch = typeof store.dispatch
export default store
export type AppThunk<ReturnType = void> = ThunkAction<
  ReturnType,
  RootState,
  unknown,
  Action<string>
>
export const useAppDispatch = useDispatch.withTypes<AppDispatch>()
export const useAppSelector = useSelector.withTypes<RootState>()
