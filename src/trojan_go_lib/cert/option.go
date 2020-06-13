package cert

import (
	"github.com/p4gefau1t/trojan-go/common"
	"github.com/p4gefau1t/trojan-go/log"
)

type certOption struct {
	mode     *string
	httpPort *string
	tlsPort  *string
	common.OptionHandler
}

func (*certOption) Name() string {
	return "cert"
}

func (*certOption) Priority() int {
	return 10
}

func (c *certOption) Handle() error {
	tlsPort = *c.tlsPort
	httpPort = *c.httpPort
	switch *c.mode {
	case "request":
		RequestCertGuide()
		return nil
	case "renew":
		RenewCertGuide()
		return nil
	case "INVALID":
		return common.NewError("not specified")
	default:
		err := common.NewError("invalid args " + *c.mode)
		log.Error(err)
		return common.NewError("invalid args")
	}
}
