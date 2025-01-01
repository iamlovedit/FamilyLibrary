import { useEffect, useState } from 'react'
import { useQuery } from 'react-query'

import { getPackagePage } from '@/http/dynamo'
type QueryParameters = {
  keyword?: string
  page?: number
  pageSize?: number
}
function DynamoBroswer() {
  const [queryParameters, setQueryParameters] = useState<QueryParameters>({
    page: 1,
    pageSize: 30
  })

  useEffect(() => {
    getPackagePage(queryParameters.page, queryParameters.pageSize)
      .then((response) => {
        console.log(response.response)
      })
      .catch((error: any) => {})
  }, [queryParameters])
  return <div>{}</div>
}

export default DynamoBroswer
