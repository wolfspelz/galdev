using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;

namespace n3q.Tools
{
    public class Sax
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Really?")]
        public class AttributeSet : Dictionary<string, string>
        {
            public string Get(string key, string defaultValue)
            {
                if (this.ContainsKey(key)) {
                    return this[key];
                }
                return defaultValue;
            }
        }

        public ICallbackLogger Log { get; set; } = new NullCallbackLogger();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Really?")]
        public class PreambleArgs : EventArgs
        {
            public string Name { get; set; }
            public AttributeSet Attributes { get; set; }
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Really?")]
        public class StartElementArgs : EventArgs
        {
            public string Name { get; set; }
            public int Depth { get; set; }
            public AttributeSet Attributes { get; set; }
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Really?")]
        public class EndElementArgs : EventArgs
        {
            public string Name { get; set; }
            public int Depth { get; set; }
            public string Xml { get; set; }
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Really?")]
        public class CharacterDataArgs : EventArgs
        {
            public string Text { get; set; }
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Really?")]
        public class CurrentCharacterArgs : EventArgs
        {
            public int Depth { get; set; }
            public char C { get; set; }
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Really?")]
        public class ParseErrorArgs : EventArgs
        {
            public int Column { get; set; }
            public int Line { get; set; }
            public string Message { get; set; }
            public string Vicinity { get; set; }
        }

        public event EventHandler<PreambleArgs> Preamble;
        public event EventHandler<StartElementArgs> StartElement;
        public event EventHandler<EndElementArgs> EndElement;
        public event EventHandler<CharacterDataArgs> CharacterData;
        public event EventHandler<CurrentCharacterArgs> CurrentCharacter;
        public event EventHandler<ParseErrorArgs> ParseError;

        public void Parse(byte[] bytes)
        {
            var s = NextString(bytes, Encoding.UTF8);
            Parse(s);
        }

        Decoder _decoder;
        public string NextString(byte[] bytes, Encoding encoding)
        {
            //return Encoding.UTF8.GetString(bytes);

            _decoder ??= encoding.GetDecoder();
            StringBuilder sb = new StringBuilder();

            char[] chars = new char[encoding.GetMaxCharCount(bytes.Length)];
            int readChars = _decoder.GetChars(bytes, 0, bytes.Length, chars, 0);
            if (readChars > 0) {
                sb.Append(chars, 0, readChars);
            }

            return sb.ToString();
        }

        public enum State
        {
            BeforeRoot,
            TagName,
            Attributes,
            Text,
            Tag,
            ClosingTag,
            Comment,
        }

        State _state = State.BeforeRoot;

        int _slashFlag = -1;
        int _openingFlag = -1;
        readonly Stack<string> _tagStack = new Stack<string>();
        string _tagName = "";
        string _tagText = "";
        string _attributes = "";
        string _closingName = "";

        int _charIndex = 0;
        int _rowNumber = 1;
        int _columnNumber = 0;
        string _errorText = "";
        bool _isError;

        string _comment = "";

        private bool JustHadSlash => _slashFlag == 0;
        private bool JustHadOpeningBracket => _openingFlag == 0;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "really?")]
        public void Parse(string data)
        {
            _charIndex = -1;
            _columnNumber = 0;

            foreach (var c in data) {
                _slashFlag--; if (_slashFlag < -1) { _slashFlag = -1; }
                _openingFlag--; if (_openingFlag < -1) { _openingFlag = -1; }
                _charIndex++;
                _columnNumber++;
                if (c == '\n') {
                    _rowNumber++;
                    _columnNumber = 0;
                }

                if (Log.IsFlooding()) { Log.Flooding($"BEFORE c={c} #{_charIndex} row={_rowNumber} col={_columnNumber} state={_state} stack.Count={_tagStack.Count} stack[0]={(_tagStack.Count > 0 ? _tagStack.Peek() : "")} tagName={_tagName} tagText={_tagText} attrib={_attributes} closingName={_closingName} error={_errorText} isError={_isError} comment={_comment}"); }

                try { CurrentCharacter?.Invoke(this, new CurrentCharacterArgs { Depth = _tagStack.Count, C = c }); } catch (Exception ex) { _ = ex; }

                switch (_state) {
                    case State.BeforeRoot:
                        switch (c) {
                            case '<': BeginTag(); break;
                            case '>': Error($"'{c}' in {_state}"); break;
                            default: break;
                        }
                        break;

                    case State.TagName:
                        switch (c) {
                            case '<': Error($"'{c}' in {_state}"); break;
                            case '>':
                                if (string.IsNullOrEmpty(_tagName)) {
                                    Error($"'{c}' in {_state}");
                                } else {
                                    try { StartElement?.Invoke(this, new StartElementArgs { Name = _tagName, Depth = _tagStack.Count, Attributes = GetAttributes(_attributes), }); } catch (Exception ex) { _ = ex; }
                                    if (JustHadSlash) {
                                        EndTag();
                                    } else {
                                        _state = State.Text;
                                        _tagText = "";
                                    }
                                }
                                break;
                            case ' ':
                                _state = State.Attributes;
                                break;
                            case '/':
                                if (JustHadOpeningBracket) {
                                    _state = State.ClosingTag;
                                    _tagName = _tagStack.Pop();
                                } else {
                                    _slashFlag = 1;
                                }
                                break;
                            default:
                                _tagName += c;
                                if (_tagName == "!--") {
                                    _state = State.Comment;
                                    _comment = "<" + _tagName;
                                    _tagName = "";
                                }
                                break;
                        }
                        break;

                    case State.Comment:
                        _comment += c;
                        if (_comment.EndsWith("-->")) {
                            _state = State.Text;
                        }
                        break;

                    case State.Attributes:
                        switch (c) {
                            case '<': Error($"'{c}' in {_state}"); break;
                            case '/':
                                _slashFlag = 1;
                                _attributes += c;
                                break;
                            case '>':
                                if (_tagName.StartsWith("?") && _tagName.ToLower() == "?xml") {
                                    if (_attributes.EndsWith("?")) { _attributes = _attributes.Substring(0, _attributes.Length - 1); }
                                    try { Preamble?.Invoke(this, new PreambleArgs { Name = _tagName, Attributes = GetAttributes(_attributes) }); } catch (Exception ex) { _ = ex; }
                                    _state = State.BeforeRoot;
                                    _tagName = "";
                                } else {
                                    if (_attributes.EndsWith("/")) { _attributes = _attributes.Substring(0, _attributes.Length - 1); }
                                    try { StartElement?.Invoke(this, new StartElementArgs { Name = _tagName, Depth = _tagStack.Count, Attributes = GetAttributes(_attributes), }); } catch (Exception ex) { _ = ex; }
                                    if (JustHadSlash) {
                                        EndTag();
                                    }
                                    _state = State.Text;
                                    _tagText = "";
                                }
                                break;
                            default:
                                _attributes += c;
                                break;
                        }
                        break;

                    case State.Text:
                        switch (c) {
                            case '<':
                                BeginTag();
                                _openingFlag = 1;
                                break;
                            case '>': Error($"'{c}' in {_state}"); break;
                            default:
                                _tagText += c;
                                break;
                        }
                        break;

                    case State.Tag:
                        switch (c) {
                            case '<': Error($"'{c}' in {_state}"); break;
                            case '>': _state = State.BeforeRoot; break;
                            case ' ': Error($"'{c}' in {_state}"); break;
                            case '/':
                                _state = State.ClosingTag;
                                _closingName = "";
                                break;

                            default: break;
                        }
                        break;

                    case State.ClosingTag:
                        switch (c) {
                            case '<': Error($"'{c}' in {_state}"); break;
                            case '/': Error($"'{c}' in {_state}"); break;
                            case '>':
                                if (string.IsNullOrEmpty(_tagName)) {
                                    Error("Tag name empty");
                                } else {
                                    if (_tagName == _closingName) {
                                        EndTag();
                                    } else {
                                        Error($"Tag name mismatch {_tagName} != {_closingName}");
                                    }
                                }
                                break;
                            case ' ': Error($"'{c}' in {_state}"); break;

                            default:
                                _closingName += c;
                                break;
                        }
                        break;
                }

                if (Log.IsFlooding()) { Log.Flooding($"AFTER  c={c} #{_charIndex} row={_rowNumber} col={_columnNumber} state={_state} stack.Count={_tagStack.Count} stack[0]={(_tagStack.Count > 0 ? _tagStack.Peek() : "")} tagName={_tagName} tagText={_tagText} attrib={_attributes} closingName={_closingName} error={_errorText} isError={_isError} comment={_comment}"); }

                if (_isError) {
                    try {
                        ParseError?.Invoke(this, new ParseErrorArgs {
                            Column = _columnNumber,
                            Line = _rowNumber,
                            Message = _errorText,
                            Vicinity = data,
                        });
                    } catch (Exception ex) { _ = ex; }
                    return;
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "really?")]
        private void BeginTag()
        {
            if (!string.IsNullOrEmpty(_tagName)) {
                _tagStack.Push(_tagName);
                _tagName = "";
            }
            if (!string.IsNullOrEmpty(_tagText)) {
                try { CharacterData?.Invoke(this, new CharacterDataArgs { Text = GetText(_tagText), }); } catch (Exception ex) { _ = ex; }
                _tagText = "";
            }
            _attributes = "";
            _state = State.TagName;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "really?")]
        private void EndTag()
        {
            try { EndElement?.Invoke(this, new EndElementArgs { Name = _tagName, Depth = _tagStack.Count }); } catch (Exception ex) { _ = ex; }
            _state = State.Text;
            _tagName = "";
            _closingName = "";
            _attributes = "";
        }

        private string GetText(string text)
        {
            return HttpUtility.HtmlDecode(text);
        }

        private AttributeSet GetAttributes(string attributes)
        {
            var dict = new AttributeSet();

            var attribs = attributes.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var attrib in attribs) {
                if (attrib.StartsWith("=")) {
                    Error($"Invalid attribute '{attrib}'");
                    break;
                }

                var kv = attrib.Split('=', 2, StringSplitOptions.RemoveEmptyEntries);
                if (kv.Length == 0 || string.IsNullOrEmpty(kv[0])) {
                    Error($"Invalid attribute '{attrib}'");
                    break;
                }

                if (kv.Length == 1) {
                    dict[kv[0]] = "";
                } else if (kv.Length == 2) {
                    dict[kv[0]] = HttpUtility.HtmlDecode(kv[1].Trim('"').Trim('\''));
                }
            }

            return dict;
        }

        private void Error(string text, [CallerLineNumber] int sourceNumber = 0)
        {
            _errorText = $"Unexpected character in {_state} source line={sourceNumber} row={_rowNumber} column={_columnNumber} char={_charIndex}: {text}";
            _isError = true;
        }
    }
}
