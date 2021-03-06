﻿using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WebBrowserLaboratory.Helpers.Web
{
    class BaseHtmlHelper
    {
        #region DOM Element Helpers
        /// <summary>
        /// Obtener listado de frames de HtmlDocument.Window.Frames
        /// </summary>
        /// <param name="pHtmlDoc">HtmlDocument donde se va a buscar</param>
        /// <returns>HtmlWindowCollection listado de frames</returns>
        public static HtmlWindowCollection GetFramesFromDocument(HtmlDocument pHtmlDoc)
        {
            try
            {
                return pHtmlDoc != null && pHtmlDoc.Window != null && pHtmlDoc.Window.Frames != null && pHtmlDoc.Window.Frames.Count > 0
                    ? pHtmlDoc.Window.Frames
                    : null;
            }
            catch (Exception vE)
            {
                throw vE;
            }
        }

        /// <summary>
        /// Evaluar si contiene HtmlDocument.Body.InnerHtml
        /// </summary>
        /// <param name="pHtmlDoc">HtmlDocument donde se va a buscar</param>
        /// <returns>Boolean</returns>
        public static bool BodyHasHtmlFromDocument(HtmlDocument pHtmlDoc)
        {
            try
            {
                return pHtmlDoc != null && pHtmlDoc.Body != null && !string.IsNullOrWhiteSpace(pHtmlDoc.Body.InnerHtml);
            }
            catch (Exception vE)
            {
                throw vE;
            }
        }

        /// <summary>
        /// Evaluar si el frame (objeto HtmlWindow) es distinto de núlo y contenga HtmlWindow.Document
        /// </summary>
        /// <param name="pFrame">Objeto HtmlWindow a evaluar</param>
        /// <returns>Boolean</returns>
        public static bool FrameIsNotNull(HtmlWindow pFrame)
        {
            try
            {
                return pFrame != null && pFrame.Document != null;
            }
            catch (Exception vE)
            {
                throw vE;
            }
        }

        /// <summary>
        /// Obtener listado de HtmlElement de HtmlDocument.Window.Frames por nombre de etiqueta
        /// </summary>
        /// <param name="pHtmlDoc">HtmlDocument donde se va a buscar</param>
        /// <param name="pTag">Nombre de la etiqueta para hacer la búsqueda</param>
        /// <returns>HtmlElementCollection listado de HtmlElement</returns>
        public static HtmlElementCollection GetHtmlElementsFromFramesByTagName(HtmlDocument pHtmlDoc, string pTag)
        {
            try
            {
                HtmlElementCollection vHtmlElements = null;
                HtmlWindowCollection vParentFrames = GetFramesFromDocument(pHtmlDoc);
                if (vParentFrames != null)
                {
                    foreach (HtmlWindow vParentFrame in vParentFrames)
                    {
                        if (vHtmlElements == null)
                        {
                            try
                            {
                                if (FrameIsNotNull(vParentFrame))
                                {
                                    vHtmlElements = vParentFrame.Document.GetElementsByTagName(pTag).Count > 0
                                        ? vParentFrame.Document.GetElementsByTagName(pTag) : null;
                                    if (vHtmlElements == null)
                                    {
                                        if (GetFramesFromDocument(vParentFrame.Document) != null)
                                            vHtmlElements = GetHtmlElementsFromFramesByTagName(vParentFrame.Document, pTag);
                                    }
                                }
                            }
                            catch (Exception vE)
                            {
                                // Continuar si falla
                                continue;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                return vHtmlElements;
            }
            catch (Exception vE)
            {
                throw vE;
            }
        }

        /// <summary>
        /// Obtener un HtmlElement de HtmlDocument.Window.Frames
        /// </summary>
        /// <param name="pHtmlDoc">HtmlDocument donde se va a buscar</param>
        /// <param name="pIdentifier">Identificador del elemento que se está buscando (id o name o attribute)</param>
        /// <returns>HtmlElement encontrado o nulo si no se encontró</returns>
        public static HtmlElement GetHtmlElementFromFrames(HtmlDocument pHtmlDoc, string pIdentifier)
        {
            try
            {
                if (pHtmlDoc == null) return null;
                HtmlElement vHtmlElement = null;
                HtmlWindowCollection vParentFrames = GetFramesFromDocument(pHtmlDoc);
                if (vParentFrames != null)
                {
                    foreach (HtmlWindow vParentFrame in vParentFrames)
                    {
                        if (vHtmlElement == null)
                        {
                            try
                            {
                                if (FrameIsNotNull(vParentFrame))
                                {
                                    vHtmlElement = vParentFrame.Document.GetElementById(pIdentifier) != null
                                    ? vParentFrame.Document.GetElementById(pIdentifier)
                                    : (vParentFrame.Document.All[pIdentifier] != null ? vParentFrame.Document.All[pIdentifier] : null);
                                    if (vHtmlElement == null)
                                    {
                                        if (GetFramesFromDocument(vParentFrame.Document) != null)
                                            vHtmlElement = GetHtmlElementFromFrames(vParentFrame.Document, pIdentifier);
                                    }
                                }
                            }
                            catch (Exception vE)
                            {
                                // Continuar si falla
                                continue;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                return vHtmlElement;
            }
            catch (Exception vE)
            {
                throw vE;
            }
        }

        /// <summary>
        /// Obtener un HtmlElement de HtmlDocument, buscando primero por 'id', luego por 'name'
        /// </summary>
        /// <param name="pHtmlDoc">HtmlDoc donde se va a buscar</param>
        /// <param name="pIdentifier">Identificador del elemento que se está buscando</param>
        /// <returns>HtmlElement encontrado o nulo si no se encontró</returns>
        public static HtmlElement GetHtmlElement(HtmlDocument pHtmlDoc, string pIdentifier)
        {
            try
            {
                if (pHtmlDoc == null || string.IsNullOrWhiteSpace(pIdentifier)) return null;
                HtmlElement vHtmlElement = pHtmlDoc.GetElementById(pIdentifier) != null
                    ? pHtmlDoc.GetElementById(pIdentifier)
                    : (pHtmlDoc.All[pIdentifier] != null ? pHtmlDoc.All[pIdentifier] : null);
                if (vHtmlElement == null) vHtmlElement = GetHtmlElementFromFrames(pHtmlDoc, pIdentifier); // Si no se encuentra el elemento, se intenta buscar en los iframes (si tiene)
                return vHtmlElement;
            }
            catch (Exception vE)
            {
                throw vE;
            }
        }

        /// <summary>
        /// Obtener un HtmlElement de HtmlDocument, buscando por 'tag', 'attribute' y 'value'
        /// </summary>
        /// <param name="pHtmlDoc">HtmlDoc donde se va a buscar</param>
        /// <param name="pTag">Nombre del tag que se está buscando</param>
        /// <param name="pAttribute">Nombre del attribute que se está buscando</param>
        /// <param name="pValue">Nombre del value que se está buscando</param>
        /// <returns>HtmlElement encontrado o nulo si no se encontró</returns>
        public static HtmlElement GetHtmlElementByAttribute(HtmlDocument pHtmlDoc, string pTag, string pAttribute, string pValue)
        {
            try
            {
                if (pHtmlDoc == null) return null;
                pTag = string.IsNullOrWhiteSpace(pTag) ? string.Empty : pTag;
                pAttribute = string.IsNullOrWhiteSpace(pAttribute) ? string.Empty : pAttribute;
                pValue = string.IsNullOrWhiteSpace(pValue) ? string.Empty : pValue;
                HtmlElement vHtmlElement = null;
                if (pHtmlDoc != null)
                {
                    foreach (HtmlElement element in pHtmlDoc.GetElementsByTagName(pTag))
                    {
                        if (element.GetAttribute(pAttribute) != null && element.GetAttribute(pAttribute).Length != 0 && element.GetAttribute(pAttribute) == pValue)
                        {
                            vHtmlElement = element;
                            break;
                        }
                    }
                }
                if (vHtmlElement == null) vHtmlElement = GetHtmlElementFromFrames(pHtmlDoc, pAttribute); // Si no se encuentra el elemento, se intenta buscar en los iframes (si tiene)
                return vHtmlElement;
            }
            catch (Exception vE)
            {
                throw vE;
            }
        }

        /// <summary>
        /// Obtiene el primer elemento que coincida con la expresión regular
        /// Si no se puede obtener el elemento por identificador de la primera coincidencia, se busca el elemento por tag de la primera coincidencia
        /// </summary>
        /// <param name="pHtmlDoc">HtmlDocument donde se va a buscar</param>
        /// <param name="pHtml">HTML string para evaluar pRegexExpression</param>
        /// <param name="pRegexExpression">Expersión regular para buscar el elemento</param>
        /// <param name="pTag">Opcional: nombre del tag HTML para buscar el elemento</param>
        /// <returns>Si se encuentra el elemento se retorna el objeto HtmlElement, de lo contrario se retorna null</returns>
        public static HtmlElement GetHtmlElementByRegexExpression(HtmlDocument pHtmlDoc, string pHtml, string pRegexExpression, string pTag = null)
        {
            try
            {
                if (pHtmlDoc == null || string.IsNullOrWhiteSpace(pHtml)) return null;
                pRegexExpression = string.IsNullOrWhiteSpace(pRegexExpression) ? string.Empty : pRegexExpression;
                Match vMatch = Regex.Match(pHtml.RemoveTildes(), pRegexExpression, RegexOptions.IgnoreCase);
                if (!vMatch.Success) return null;
                string vFirstCoincidence = vMatch.Groups[0].Value;  // Primera coincidencia
                string vIdentifier = vFirstCoincidence.GetIdentifier(); // Busca el identificador de la primera coincidencia
                HtmlElement vHtmlElement = null;
                if (!string.IsNullOrEmpty(vIdentifier)) // Si se obtuvo el identificador
                {
                    vHtmlElement = GetHtmlElement(pHtmlDoc, vIdentifier);   // Obtener el elemento por identificador
                }
                else if (!string.IsNullOrEmpty(pTag))   // Si no se obtuvo el identificador, pero se conoce el tag del elemento
                {
                    HtmlElementCollection vHtmlElements = pHtmlDoc.GetElementsByTagName(pTag);
                    if (vHtmlElements == null || vHtmlElements.Count == 0) vHtmlElements = GetHtmlElementsFromFramesByTagName(pHtmlDoc, pTag);
                    if (vHtmlElements != null && vHtmlElements.Count > 0)
                    {
                        foreach (HtmlElement vElement in vHtmlElements)
                        {
                            if (Regex.IsMatch(vElement.OuterHtml.RemoveTildes(), pRegexExpression, RegexOptions.IgnoreCase))  // Si elemento por tag es igual a la primera coincidencia
                            {
                                vHtmlElement = vElement;    // Obtener el elemento por tag
                                break;
                            }
                        }
                    }
                }
                return vHtmlElement;
            }
            catch (Exception vE)
            {
                throw vE;
            }
        }

        /// <summary>
        /// Obtener el string del HTML de un HtmlElement que se obtiene por identificador.
        /// Si no está definido el identificador para obtener el HtmlElement, entonces el string del HTML se toma de HtmlDocument.
        /// Si se encuentra un HtmlElement con el identificador definido y el HtmlElement es un IFRAME, entonces el string del HTML se toma de HtmlDocument del IFRAME.
        /// </summary>
        /// <param name="pHtmlDoc">HtmlDocument que proviene del WebBrowser actual. Se utiliza para obtener el string del HTML.</param>
        /// <param name="pElementIdentifier">Opcional: Identificador para buscar el HtmlElement.</param>
        /// <returns>String del HTML de un HtmlElement o de HtmlDocument del WebBrowser.</returns>
        public static string GetHtmlFromElementByIdentifier(HtmlDocument pHtmlDoc, string pElementIdentifier = null)
        {
            try
            {
                if (pHtmlDoc == null) return string.Empty;
                string vHtml = string.Empty;
                if (string.IsNullOrWhiteSpace(pElementIdentifier))
                {
                    // Si no está definido el identificador para obtener el HtmlElement, entonces el string del HTML se toma de HtmlDocument del WebBrowser
                    vHtml = BodyHasHtmlFromDocument(pHtmlDoc) ? pHtmlDoc.Body.InnerHtml : string.Empty;
                }
                else
                {
                    // Si está definido el identificador para obtener el HtmlElement, entonces se busca dentro de HtmlDocument del WebBrowser
                    HtmlElement vHtmlElement = GetHtmlElement(pHtmlDoc, pElementIdentifier);
                    if (vHtmlElement != null)
                    {
                        // Si se logró obtener el HtmlElement, entonces se toma el string del HTML
                        vHtml = vHtmlElement.TagName.Equals(ElementHelper.IFRAME_NAME_TAG)
                            ? GetHtmlFromFrameByIdentifier(pHtmlDoc, pElementIdentifier) // Si el HtmlElement es iframe, entonces se busca el HTML dentro del iframe
                            : (!string.IsNullOrWhiteSpace(vHtmlElement.InnerHtml) ? vHtmlElement.InnerHtml : string.Empty);
                    }
                    else
                    {
                        // Si no se logró obtener ningún HtmlElement, entonces se devuelve una cadena vacía para esperar hasta volver a intentar buscar un elemento
                        vHtml = string.Empty;
                    }
                }
                return vHtml;
            }
            catch (NullReferenceException vE)
            {
                // No se obtuvo ningún elemento, entonces se devuelve una cadena vacía
                return string.Empty;
            }
            catch (Exception vE)
            {
                // Error desconocido, se devuelve la excepción
                throw vE;
            }
        }

        /// <summary>
        /// Obtener el string HTML de un IFrame. Se busca recursivamente dentro de child frames o sub-frames
        /// </summary>
        /// <param name="pHtmlDoc">HtmlDocument donde se va a buscar</param>
        /// <param name="pFrameIdentifier">Identificador del IFrame HTML</param>
        /// <returns>String del HTML del IFrame, si no se encuentra se obtiene string.Empty</returns>
        public static string GetHtmlFromFrameByIdentifier(HtmlDocument pHtmlDoc, string pFrameIdentifier = null)
        {
            try
            {
                if (pHtmlDoc == null) return string.Empty;
                string vHtml = string.Empty;
                // Evaluar si el HtmlDocument tiene frames
                HtmlWindowCollection vParentFrames = GetFramesFromDocument(pHtmlDoc);
                if (vParentFrames != null)
                {
                    // Recorrer los frames del HtmlDocument
                    foreach (HtmlWindow vParentFrame in vParentFrames)
                    {
                        if (FrameIsNotNull(vParentFrame))
                        {
                            // Si se conoce el Id del frame, entonces se busca por Id, de lo contrario se busca para todos los frames
                            bool vByIdentifier = !string.IsNullOrWhiteSpace(pFrameIdentifier) ? vParentFrame.WindowFrameElement.Id.Equals(pFrameIdentifier) : true;
                            if (vByIdentifier)
                            {
                                // Obtener el HTML del frame
                                vHtml = BodyHasHtmlFromDocument(vParentFrame.Document) ? vParentFrame.Document.Body.InnerHtml : vHtml;
                                break;
                            }
                            // Evaluar si el HtmlDocument del frame tiene frames (child frames o sub-frames)
                            else if (GetFramesFromDocument(vParentFrame.Document) != null)
                            {
                                // Buscar recursivamente el HTML dentro de los child frames
                                vHtml = GetHtmlFromFrameByIdentifier(vParentFrame.Document, pFrameIdentifier);
                            }
                            else
                            {
                                vHtml = string.Empty;
                            }
                        }
                        else
                        {
                            vHtml = string.Empty;
                        }
                    }
                }
                else
                {
                    vHtml = string.Empty;
                }
                return vHtml;
            }
            catch (NullReferenceException vE)
            {
                return string.Empty;
            }
            catch (Exception vE)
            {
                throw vE;
            }
        }
        #endregion
    }
}