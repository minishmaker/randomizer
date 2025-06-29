.thumb
cmp	r0,#3
bls	fillWallet

mov	r0,#3
strb	r0,[r1]
b	return

fillWallet:
ldr	r1,=#0x80FCAD4 @ gWalletSizes
lsl     r0,#2
ldrh	r2,[r1,r0]
sub	r0,#4
ldrh	r1,[r1,r0]
sub	r0,r2,r1

ldr	r3,=#0x80522E8 @ ModRupees
mov	lr,r3
.short	0xF800

return:
ldr	r3,=#0x8053E15
bx	r3
