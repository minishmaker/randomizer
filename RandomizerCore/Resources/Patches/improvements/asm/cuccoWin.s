.thumb

@check if we won
ldrh	r0,[r1,#0x02] @ current cuccos
ldrh	r3,[r1,#0x04] @ required cuccos
cmp	r0,r3
blo	normal

won:
mov	r0,#0
b	end

normal:
ldrh	r0,[r1]
sub	r0,#1

end:
strh	r0,[r1]
lsl	r0,#0x10
ldr	r3,=#0x80A0B99
bx	r3
