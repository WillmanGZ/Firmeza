import { ref } from 'vue';
import Cookies from 'js-cookie';

const TOKEN_KEY = 'USER_TOKEN';
const userToken = ref<string | null>(null);
const stored = Cookies.get(TOKEN_KEY);

if (stored) {
  try {
    userToken.value = JSON.parse(stored) as string;
  } catch {
    userToken.value = null;
  }
}

export function useAuth() {
  const setUserInfo = (token: string) => {
    Cookies.set(TOKEN_KEY, JSON.stringify(token), {
      expires: 7,
      sameSite: 'strict',
    });

    userToken.value = token;
  };

  const getUserInfo = (): string | null => {
    const cookie = Cookies.get(TOKEN_KEY);
    if (!cookie) return null;

    try {
      return JSON.parse(cookie) as string;
    } catch {
      return null;
    }
  };

  const removeUserInfo = () => {
    Cookies.remove(TOKEN_KEY);
    userToken.value = null;
  };

  const isAuthenticated = () => {
    return !!userToken.value;
  };

  return {
    userToken,
    setUserInfo,
    getUserInfo,
    removeUserInfo,
    isAuthenticated,
  };
}
