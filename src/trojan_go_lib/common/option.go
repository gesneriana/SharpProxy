package common

type OptionHandler interface {
	Name() string
	Handle() error
	Priority() int
}

var Handlers = make(map[string]OptionHandler)

func RegisterOptionHandler(h OptionHandler) {
	Handlers[h.Name()] = h
}

func PopOptionHandler() (OptionHandler, error) {
	var maxHandler OptionHandler = nil
	for _, h := range Handlers {
		if maxHandler == nil || maxHandler.Priority() < h.Priority() {
			maxHandler = h
		}
	}
	if maxHandler == nil {
		return nil, NewError("no option left")
	}
	delete(Handlers, maxHandler.Name())
	return maxHandler, nil
}
