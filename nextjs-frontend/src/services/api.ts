import Axios from 'axios';
import Urls from '../url-constants'

export default () => {
  return Axios.create({
    baseURL: Urls.ApiURL

  })
}