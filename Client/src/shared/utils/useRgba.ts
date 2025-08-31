const isValidHex = (hex: string) => /^#([A-Fa-f0-9]{3,4}){1,2}$/.test(hex);

const getChunksFromString = (st: string, chunkSize: number) => st.match(new RegExp(`.{${chunkSize}}`, 'g'))!;

const convertHexUnitTo256 = (hexStr: string) => parseInt(hexStr.repeat(2 / hexStr.length), 16);

const getAlphafloat = (a: number, alpha: number) => {
  if (typeof a !== 'undefined') {
    return a / 255;
  }
  if (typeof alpha != 'number' || alpha < 0 || alpha > 1) {
    return 1;
  }
  return alpha;
};

/**
 * Преобразует HEX цвет в RGBA
 * @param hex - строка в формате HEX (#RGB, #RGBA, #RRGGBB, #RRGGBBAA)
 * @param alpha - значение прозрачности от 0 до 1 (используется, если в hex нет альфа-канала)
 * @returns строка в формате RGBA
 * @throws Error - если передан некорректный HEX
 * @example
 * hexToRGBA('#f80') // 'rgba(255, 136, 0, 1)'
 * hexToRGBA('#f808') // ' rgba(255, 136, 0, 0.53125)'
 * hexToRGBA('#0088ff') // 'rgba(0, 136, 255, 1)'
 * hexToRGBA('#0088ff88') // 'rgba(0, 136, 255, 0.53125)'
 * hexToRGBA('#98736') // Uncaught Error: Invalid HEX
 */
export const hexToRGBA = (hex: string, alpha: number = 1) => {
  if (!isValidHex(hex)) {
    console.error(`Invalid HEX color: ${hex}`);
    throw new Error('Invalid HEX');
  }
  const chunkSize = Math.floor((hex.length - 1) / 3);
  const hexArr = getChunksFromString(hex.slice(1), chunkSize);
  const [r, g, b, a] = hexArr.map(convertHexUnitTo256);
  return `rgba(${r}, ${g}, ${b}, ${getAlphafloat(a, alpha)})`;
};
