.thumb
cmp	r0,#3
bls	fillWallet

mov	r0,#3
strb	r0,[r1]

fillWallet:
ldr	r0,=#9999
ldr	r3,=#0x80522E8 @ ModRupees
mov	lr,r3
.short	0xF800

return:
ldr	r3,=#0x8053E15
bx	r3
